using System;
using System.IO;
using System.Threading;
using RagnaCustoms.Services;

namespace RagnaCustoms.App.Services
{
    public sealed class FileChangeEvent : IDisposable
    {
        private FileSystemWatcher _watcher;
        private Action<object, FileSystemEventArgs> _onChangeEvent;
        private int _disposed;

        public FileChangeEvent(string path, string filter)
        {
            try
            {
                _watcher = new FileSystemWatcher(path, filter)
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    IncludeSubdirectories = true,
                    EnableRaisingEvents = true
                };
                _watcher.Changed += OnChange;
                _watcher.Error += OnError;
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Unable to watch Ragnarock log directory.", exception);
            }
        }

        public void SetLambda(Action<object, FileSystemEventArgs> onChange)
        {
            _onChangeEvent = onChange;
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;

            var callback = _onChangeEvent;
            if (callback == null) return;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    callback(sender, e);
                }
                catch (Exception exception)
                {
                    TwitchBotLogger.Error("Unhandled exception while processing Ragnarock log change.", exception);
                }
            });
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            var exception = e.GetException();
            if (exception != null)
                TwitchBotLogger.Error("FileSystemWatcher reported an error.", exception);
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
            if (_watcher == null) return;

            _watcher.EnableRaisingEvents = false;
            _watcher.Changed -= OnChange;
            _watcher.Error -= OnError;
            _watcher.Dispose();
        }
    }
}
