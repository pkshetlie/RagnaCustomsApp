using System;
using System.IO;
using System.Threading;
using RagnaCustoms.App.Views;

namespace RagnaCustoms.App.Services
{
    public class FileChangeEvent
    {
        private string _path;
        private Action<object, FileSystemEventArgs, FileChangeEvent> _onChangeEvent;
        private FileSystemWatcher _watcher;
        public TwitchBotForm _twitchBotForm = null;
        
        public FileChangeEvent(string path, string filter)
        {
            using var watcher = new FileSystemWatcher(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = filter;
            watcher.Changed += OnChange;
            watcher.Error += OnError;
            
            _path = path;
            _watcher = watcher;
        }
        public void SetTwitchBotForm(TwitchBotForm form)
        {
            _twitchBotForm = form;
        }

        public void SetLambda(Action<object, FileSystemEventArgs, FileChangeEvent> onChange)
        {
            _onChangeEvent = onChange;
        }
        
        private void OnChange(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;
            var me = this;
            new Thread(() => { _onChangeEvent(sender, e, me); }).Start();
        }
        private static void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());
        private static void PrintException(Exception? ex) 
        { 
            if (ex != null) 
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}