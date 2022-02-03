using System;
using System.IO;
using System.Threading;
using RagnaCustoms.App.Views;

namespace RagnaCustoms.App.Services
{
    public class FileChangeEvent
    {
        private Action<object, FileSystemEventArgs> _onChangeEvent;
        
        public FileChangeEvent(string path, string filter)
        {
            new Thread(() =>
            {
                using var watcher = new FileSystemWatcher($@"{path}", filter);
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += new FileSystemEventHandler(OnChange);
                watcher.Error += new ErrorEventHandler(OnError);
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                while (true);
            }).Start();
        }

        public void SetLambda(Action<object, FileSystemEventArgs> onChange)
        {
            _onChangeEvent = onChange;
        }
        
        private void OnChange(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;
            new Thread(() => { _onChangeEvent(sender, e); }).Start();
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