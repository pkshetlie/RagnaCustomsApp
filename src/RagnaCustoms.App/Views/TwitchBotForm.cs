﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using RagnaCustoms.App.Services;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace RagnaCustoms.App.Views
{
    public partial class TwitchBotForm : Form
    {
        private readonly List<string> _allowedPrefixes = new()
        {
            "!rc", "!rcs", "!rcr"
        };

        public bool botEnabled = false;

        public Configuration _configuration;
        private JoinedChannel _joinedChannel;

        private readonly List<Song> _songList = new();
        private bool _twitchBotEnabled;

        private TwitchClient _twitchClient;

        public Dictionary<string, ICommandes> Commandes = new();

        public bool QueueIsOpen = true;
        private string _lastPlayedHash = string.Empty;

        private void OnFileChange(object sender, FileSystemEventArgs e)
        {

            var songLevelLineHint = "LogTemp: Warning: Song level str";
            var songNameLineHint = "LogTemp: Loading song";
            var songScoreDetailLineHint = "LogTemp: Warning: Notes missed :";
            var songScoreLineHint = "raw distance =";

            var session = new Session();

            if (!File.Exists(e.FullPath)) return;
            using var stream = File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream);
            List<string> lines = new List<string>();
            while (!reader.EndOfStream) lines.Add(reader.ReadLine());
            foreach (var line in lines)
            {
                if (line.Contains(songLevelLineHint))
                {
                    session = new Session();
                    session.Song.Level = line.Substring(line.IndexOf(songLevelLineHint) + songLevelLineHint.Length).Trim(' ', '.');
                }
                else if (line.Contains(songNameLineHint))
                {
                    var songOggPath = line.Substring(line.IndexOf(songNameLineHint) + songNameLineHint.Length)
                        .Trim(' ', '.');
                    var songDirectoryPath = Path.GetDirectoryName(songOggPath);
                    var songDirectory = new DirectoryInfo(songDirectoryPath);
                    if (!songDirectory.Exists) continue;
                    var songDatFiles = songDirectory.EnumerateFiles("*.dat");
                    var filesHashs = songDatFiles.Select(ComputeMd5).OrderBy(hash => hash);
                    var concatenatedHashs = string.Concat(filesHashs);
                    session.Song.Hash = ComputeMd5(concatenatedHashs);
                    _lastPlayedHash = session.Song.Hash;
                }
                else if (line.Contains(songScoreLineHint))
                {
                    var startIndex = line.IndexOf(songScoreLineHint) + songScoreLineHint.Length;
                    var endIndex = line.IndexOf("and adjusted distance =");
                    session.Score = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');
                    if (session.Song.Hash is null) continue;
                }
            }
            if (session.Score is null) return;
            if (session.Song.Hash is null) return;
            if (_songList.All(song => song.Hash != session.Song.Hash)) return;
            Thread.Sleep(2000);
            RemoveAtSongRequestInList(session.Song.Hash);
        }

        protected virtual string ComputeMd5(FileInfo file)
        {
            using var md5 = MD5.Create();
            using var stream = file.OpenRead();

            var hash = md5.ComputeHash(stream);
            var hashStr = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            return hashStr;
        }
        protected virtual string ComputeMd5(string str)
        {
            using var md5 = MD5.Create();

            var strBytes = Encoding.Default.GetBytes(str);
            var hash = md5.ComputeHash(strBytes);
            var hashStr = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            return hashStr;
        }

        public TwitchBotForm()
        {
            InitializeComponent();

            _configuration = new Configuration();

            botEnabled = _configuration.TwitchBotAutoStart;
            _twitchBotEnabled = _configuration.TwitchBotAutoStart;
            EnableButton.Text = botEnabled ? "Stop" : "Start";
            new FileChangeEvent(Program.RagnarockSongLogsDirectoryPath, "Ragnarock.log").SetLambda(OnFileChange);

            LoadCommands();
            checkEnabled();
        }

        public Process RagnarockApp { get; set; }


        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://twitchtokengenerator.com/");
            Process.Start(sInfo);
        }

        public void checkEnabled()
        {

            if (botEnabled &&
                (string.IsNullOrEmpty(_configuration.AuthTmi) || string.IsNullOrEmpty(_configuration.TwitchChannel)))
            {
                MessageBox.Show("You need to configure in preference before enable", "RagnaCustoms.com", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                //MessageBox.Show(Resources.app.strings.Error_2, "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                botEnabled = false;
                return;
            }

            if (botEnabled)
            {
                var credentials = new ConnectionCredentials(_configuration.TwitchChannel, _configuration.AuthTmi);
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };

                _twitchClient = new TwitchClient(new WebSocketClient(clientOptions));

                _twitchClient.Initialize(credentials, _configuration.TwitchChannel);
                _twitchClient.OnMessageReceived += OnMessageReceived;
                _twitchClient.OnUserBanned += OnUserBanned;

                _twitchClient.OnConnected += OnConnected;
                _twitchClient.Connect();
            }

            _twitchBotEnabled = botEnabled;
        }

        private void OnUserBanned(object sender, OnUserBannedArgs e)
        {
            RemoveSongByRequester(e.UserBan.Username);
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            _joinedChannel = _twitchClient.GetJoinedChannel(_configuration.TwitchChannel);

            if (!_configuration.DisableBotWelcome)
            {
                _twitchClient.SendMessage(_joinedChannel, $"{_configuration.BotPrefix} Ragnacustoms.com's bot connected");
            }
            //TwitchClient.SendMessage(joinedChannel, $"{prefixe}{Resources.app.strings.WelcomeBot}");
        }

        private void LoadCommands()
        {
            var type = typeof(ICommandes);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (var command in types)
                try
                {
                    var cmd = Activator.CreateInstance(command) as ICommandes;
                    if (cmd != null)
                        foreach (var name in cmd.Names())
                            Commandes.Add(name, cmd);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }

        private void StartDownload(string v)
        {
            var songId = v;
            var songProvider = new SongProvider();
            var downloadingView = new DownloadingForm();
            var downloadingPresenter = new DownloadingPresenter(downloadingView, songProvider);
            downloadingPresenter.Download(songId, true, null, _configuration.RequestFolder);
            Application.Run(downloadingView);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!_twitchBotEnabled) return;

            var command = e.ChatMessage.Message.Split(' ');

            if (!_allowedPrefixes.Contains(command[0])) return;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (_configuration.ViewerLang != null && _configuration.ViewerLang.ContainsKey(e.ChatMessage.UserId))
                {
                    Thread.CurrentThread.CurrentUICulture =
                        new CultureInfo(_configuration.ViewerLang[e.ChatMessage.UserId], true);
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                }

                if (command.Length < 2)
                {
                    if (!Commandes.ContainsKey("")) return;
                    var cmd = Commandes[""];
                    var success = cmd.Action(_joinedChannel, _twitchClient, this, e);
                    if (!success) _twitchClient.SendMessage(_joinedChannel, $"{_configuration.BotPrefix}An error has occurred !");
                }
                else
                {
                    var arg1 = command[1];
                    if (!Commandes.ContainsKey(arg1))
                    {
                        AddRequest(string.Join("%20", command.Skip(1).ToArray()), e);
                        return;
                    }

                    var cmd = Commandes[arg1];
                    if (cmd.IllegalUsers().Contains(e.ChatMessage.UserType))
                    {
                        _twitchClient.SendMessage(_joinedChannel,
                            $"{_configuration.BotPrefix}Vous n'avez pas l'autorisation d'éxécuter cette commande !");
                        return;
                    }

                    var success = cmd.Action(_joinedChannel, _twitchClient, this, e);
                    if (!success) _twitchClient.SendMessage(_joinedChannel, $"{_configuration.BotPrefix}An error has occurred !");
                }
            }).Start();
        }

        private void AddRequest(string requestId, OnMessageReceivedArgs e)
        {
            if (!QueueIsOpen)
            {
                _twitchClient.SendMessage(_joinedChannel, $"{_configuration.BotPrefix}Queue is closed");
                return;
            }

            var s = GetSongInfo(requestId) ?? SearshSong(requestId); // search song by id, if not found, search by name
            if (s != null)
            {
                _twitchClient.SendMessage(_joinedChannel,
                    $"{_configuration.BotPrefix}Request Info: {s.Name}, Mapped by : {s.Mapper}, asked by @{e.ChatMessage.Username}");
                AddSongRequestToList(s, e.ChatMessage.Username);
                StartDownload(s.Id.ToString());
                //TwitchClient.SendMessage(joinedChannel, $"{prefixe}Ready: ");
            }
            else
            {
                _twitchClient.SendMessage(_joinedChannel, $"{_configuration.BotPrefix}@{e.ChatMessage.Username} Song not found");
            }
        }

        private void AddSongRequestToList(Song song, string viewer)
        {
            song.Requester = viewer;
            _songList.Add(song);
            UpdateFormRows();
        }

        public void removeSongEasyStream(string songId)
        {
            var songFolder = DirProvider.getCustomDirectory()
                .GetDirectories()
                .FirstOrDefault(x =>
                {
                    return x.GetFiles().Any(z =>
                    {
                        var content = z.OpenText();
                        var toReturn = z.Name == ".id" && content.ReadToEnd() == songId.ToString();
                        content.Close();
                        return toReturn;
                    });
                });
        }
        public void RemoveSongByRequester(string viewer)
        {
            var songs = _songList.FindAll(x => x.Requester == viewer);
            if (songs.Count != 0)
            {
                songs.ForEach(x =>
                {
                    if (_configuration.EasyStreamRequest) removeSongEasyStream(x.Id);
                    _songList.Remove(x);
                });
                UpdateFormRows();
            }
        }

        public void RemoveLastPlayerSong()
        {
            RemoveAtSongRequestInList(_lastPlayedHash);
        }

        public void RemoveAtSongRequestInList(string hash)
        {
            var song = _songList.Find(s => s.Hash.Equals(hash));
            if (song == null) return;
            if (_configuration.EasyStreamRequest) removeSongEasyStream(song.Id);
            _songList.Remove(song);
            UpdateFormRows();
        }

        // set songRequest rows values to _songList values
        private void UpdateFormRows()
        {
            songRequests.Invoke(new MethodInvoker(delegate
            {
                songRequests.Rows.Clear();
                foreach (var song in _songList) songRequests.Rows.Add(song.Name, song.Author, song.Requester, song.Id);
                songRequests.Refresh();
            }));
        }

        private Song GetSongInfo(string songId)
        {
            try
            {
                using var webClient = new WebClient();
                var json = webClient.DownloadString("https://ragnacustoms.com/api/song/" + songId);
                var stuff = JsonConvert.DeserializeObject<Song>(json);
                //debug_console.Items.Add($"Début de la récuperation de {stuff.title}");
                return stuff;
            }
            catch (WebException)
            {
                return null;
            }
        }

        private Song SearshSong(string search)
        {
            try
            {
                using var webClient = new WebClient();
                var json = webClient.DownloadString("https://ragnacustoms.com/api/search/" + search);
                var stuffs = JsonConvert.DeserializeObject<SearshResult>(json);
                return stuffs.FirstResultByName(search) ?? stuffs.BestResultByName(search);
            }
            catch (WebException)
            {
                return null;
            }
        }




        private void TwitchBotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (botEnabled)
            {
                botEnabled = false;
                _twitchBotEnabled = false;
                if (_twitchClient.IsConnected)
                    _twitchClient.Disconnect();
            }
        }

        private void EnableButton_Click(object sender, EventArgs e)
        {
            botEnabled = !botEnabled;
            checkEnabled();
            EnableButton.Text = botEnabled ? "Stop" : "Start";
        }
    }
}