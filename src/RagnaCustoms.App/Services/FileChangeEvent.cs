using System;
using System.IO;

namespace RagnaCustoms.App.Services
{
    public class FileChangeEvent
    {
        private string path;
        private Action<object, FileSystemEventArgs> onChangeEvent;
        private FileSystemWatcher watcher;
        
        public FileChangeEvent(string path, string filter, Action<object, FileSystemEventArgs> onChange)
        {
            this.path = path;
            this.onChangeEvent = onChange;
            using var watcher = new FileSystemWatcher(@path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = filter;
            watcher.Changed += OnChange;
            watcher.Error += OnError;
            this.watcher = watcher;
        }
        private void OnChange(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;
            onChangeEvent(sender, e);
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