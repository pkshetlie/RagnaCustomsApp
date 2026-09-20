using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RagnaCustoms.Models;

namespace RagnaCustoms.Services
{
    internal sealed class TwitchQueuePersistence
    {
        private static readonly object Sync = new object();

        private readonly string _queueDirectoryPath;
        private readonly string _queueFilePath;

        public TwitchQueuePersistence()
        {
            _queueDirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RagnaCustoms",
                "Data");
            _queueFilePath = Path.Combine(_queueDirectoryPath, "TwitchQueue.json");
        }

        public List<Song> Load()
        {
            lock (Sync)
            {
                if (!File.Exists(_queueFilePath)) return new List<Song>();

                try
                {
                    var content = File.ReadAllText(_queueFilePath);
                    var songs = JsonConvert.DeserializeObject<List<Song>>(content);
                    return songs ?? new List<Song>();
                }
                catch (Exception exception)
                {
                    var backupPath = _queueFilePath + ".corrupt-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".json";
                    try
                    {
                        File.Copy(_queueFilePath, backupPath, false);
                    }
                    catch (Exception backupException)
                    {
                        TwitchBotLogger.Error("Unable to preserve the corrupted Twitch queue file.", backupException);
                    }

                    TwitchBotLogger.Error("Unable to load the persisted Twitch queue. Starting with an empty queue.", exception);
                    return new List<Song>();
                }
            }
        }

        public void Save(IEnumerable<Song> songs)
        {
            lock (Sync)
            {
                try
                {
                    Directory.CreateDirectory(_queueDirectoryPath);

                    var temporaryPath = _queueFilePath + ".tmp";
                    var content = JsonConvert.SerializeObject(songs, Formatting.Indented);
                    File.WriteAllText(temporaryPath, content);

                    if (File.Exists(_queueFilePath))
                        File.Replace(temporaryPath, _queueFilePath, null);
                    else
                        File.Move(temporaryPath, _queueFilePath);
                }
                catch (Exception exception)
                {
                    TwitchBotLogger.Error("Unable to persist the Twitch queue.", exception);
                }
            }
        }
    }
}
