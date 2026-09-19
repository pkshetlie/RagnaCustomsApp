using System;
using System.IO;
using System.Threading;

namespace RagnaCustoms.Services
{
    internal static class TwitchBotLogger
    {
        private static readonly object Sync = new object();

        public static readonly string LogDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "RagnaCustoms",
            "Logs");

        public static readonly string LogFilePath = Path.Combine(LogDirectoryPath, "TwitchBot.log");

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Warn(string message)
        {
            Write("WARN", message);
        }

        public static void Error(string message, Exception exception = null)
        {
            var details = exception == null
                ? message
                : message + Environment.NewLine + exception;
            Write("ERROR", details);
        }

        private static void Write(string level, string message)
        {
            try
            {
                lock (Sync)
                {
                    Directory.CreateDirectory(LogDirectoryPath);
                    var line = string.Format(
                        "[{0:O}] [{1}] [Thread {2}] {3}{4}",
                        DateTime.Now,
                        level,
                        Thread.CurrentThread.ManagedThreadId,
                        message,
                        Environment.NewLine);

                    File.AppendAllText(LogFilePath, line);
                }
            }
            catch
            {
                // Logging must never be able to stop the bot.
            }
        }
    }
}
