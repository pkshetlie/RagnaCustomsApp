using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
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
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace RagnaCustoms.App.Views
{
    public partial class TwitchBotForm : Form
    {
        private static readonly object InstanceSync = new object();
        private static TwitchBotForm _instance;

        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color BorderColor = Color.FromArgb(55, 80, 98);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);
        private static readonly Color AccentTextColor = Color.FromArgb(5, 22, 32);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private readonly List<string> _allowedPrefixes = new()
        {
            "!rc", "!rcs", "!rcr"
        };

        public bool botEnabled = false;

        public Configuration _configuration;
        private JoinedChannel _joinedChannel;

        private readonly List<Song> _songList = new();
        private readonly object _songListSync = new object();
        private bool _twitchBotEnabled;
        private bool _connectionInProgress;
        private FileChangeEvent _fileChangeEvent;

        private TwitchClient _twitchClient;

        public Dictionary<string, ICommandes> Commandes = new();

        public bool QueueIsOpen = true;
        private string _lastPlayedHash = string.Empty;

        public static void ShowInstance()
        {
            lock (InstanceSync)
            {
                if (_instance == null || _instance.IsDisposed)
                    _instance = new TwitchBotForm();

                if (!_instance.Visible)
                    _instance.Show();
                else
                {
                    if (_instance.WindowState == FormWindowState.Minimized)
                        _instance.WindowState = FormWindowState.Normal;
                    _instance.Activate();
                }
            }
        }

        private void OnFileChange(object sender, FileSystemEventArgs e)
        {
            try
            {
                var songLevelLineHint = "LogTemp: Warning: Song level str";
                var songNameLineHint = "LogTemp: Loading song";
                var songScoreLineHint = "raw distance =";

                var session = new Session();

                if (!File.Exists(e.FullPath)) return;
                using var stream = File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);
                var lines = new List<string>();
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
                        if (string.IsNullOrEmpty(songDirectoryPath)) continue;
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
                        if (endIndex < startIndex) continue;
                        session.Score = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');
                        if (session.Song.Hash is null) continue;
                    }
                }

                if (session.Score is null || session.Song.Hash is null) return;
                lock (_songListSync)
                {
                    if (_songList.All(song => song.Hash != session.Song.Hash)) return;
                }

                Thread.Sleep(2000);
                RemoveAtSongRequestInList(session.Song.Hash);
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Failed to process Ragnarock song log.", exception);
            }
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
            EnableButton.Text = GetLocalizedText(botEnabled ? "TwitchBot.Form.Stop" : "TwitchBot.Form.Start", botEnabled ? "Stop" : "Start");
            _fileChangeEvent = new FileChangeEvent(Program.RagnarockSongLogsDirectoryPath, "Ragnarock.log");
            _fileChangeEvent.SetLambda(OnFileChange);
            TwitchBotLogger.Info("Twitch bot form initialized. Auto-start: " + botEnabled);

            LoadCommands();
            checkEnabled();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            SuspendLayout();

            BackColor = WindowColor;
            FormBorderStyle = FormBorderStyle.None;
            ClientSize = new Size(900, 620);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            MaximizeBox = false;
            MinimizeBox = false;

            var titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = WindowColor,
                Padding = new Padding(18, 0, 10, 0)
            };

            var brand = new Panel
            {
                Dock = DockStyle.Left,
                Width = 58,
                BackColor = Color.Transparent
            };
            var logo = new PictureBox
            {
                Size = new Size(36, 28),
                Location = new Point(7, 9),
                Image = Properties.Resources.logocompressed,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            brand.Controls.Add(logo);

            var title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Window.TwitchBot.Title", "RagnaCustoms  ·  Twitch bot"),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = TextColor,
                Font = CreateUiFont(10f, FontStyle.Bold),
                Padding = new Padding(10, 0, 0, 0)
            };

            var closeButton = new Button
            {
                Dock = DockStyle.Right,
                Width = 42,
                Text = "×",
                FlatStyle = FlatStyle.Flat,
                BackColor = WindowColor,
                ForeColor = MutedTextColor,
                Font = CreateUiFont(15f, FontStyle.Regular),
                TabStop = false,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(151, 53, 64);
            closeButton.Click += (sender, args) => Close();

            titleBar.Controls.Add(title);
            titleBar.Controls.Add(closeButton);
            titleBar.Controls.Add(brand);
            AttachWindowDrag(titleBar);
            AttachWindowDrag(brand);
            AttachWindowDrag(logo);
            AttachWindowDrag(title);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = WindowColor,
                Padding = new Padding(22, 18, 22, 22)
            };

            var queueLabel = new Label
            {
                AutoSize = true,
                Location = new Point(164, 28),
                Text = GetLocalizedText("TwitchBot.Form.Queue", "REQUEST QUEUE"),
                ForeColor = AccentColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(9f, FontStyle.Bold)
            };

            Controls.Clear();
            Controls.Add(content);
            Controls.Add(titleBar);

            EnableButton.Location = new Point(22, 18);
            EnableButton.Size = new Size(118, 34);
            EnableButton.FlatStyle = FlatStyle.Flat;
            EnableButton.FlatAppearance.BorderSize = 0;
            EnableButton.BackColor = AccentColor;
            EnableButton.ForeColor = AccentTextColor;
            EnableButton.Font = CreateUiFont(8.5f, FontStyle.Bold);
            EnableButton.Cursor = Cursors.Hand;
            EnableButton.UseVisualStyleBackColor = false;

            songRequests.Location = new Point(22, 68);
            songRequests.Size = new Size(856, 510);
            songRequests.BackgroundColor = SurfaceColor;
            songRequests.BorderStyle = BorderStyle.FixedSingle;
            songRequests.GridColor = BorderColor;
            songRequests.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            songRequests.RowHeadersVisible = false;
            songRequests.AllowUserToAddRows = false;
            songRequests.AllowUserToResizeRows = false;
            songRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            songRequests.MultiSelect = false;
            songRequests.EnableHeadersVisualStyles = false;
            songRequests.ColumnHeadersHeight = 36;
            songRequests.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = InputColor,
                ForeColor = AccentColor,
                SelectionBackColor = InputColor,
                SelectionForeColor = AccentColor,
                Font = CreateUiFont(8f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            songRequests.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SurfaceColor,
                ForeColor = TextColor,
                SelectionBackColor = Color.FromArgb(31, 91, 116),
                SelectionForeColor = TextColor,
                Font = CreateUiFont(9f, FontStyle.Regular),
                Padding = new Padding(8, 0, 8, 0)
            };
            songRequests.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = InputColor,
                ForeColor = TextColor,
                SelectionBackColor = Color.FromArgb(31, 91, 116),
                SelectionForeColor = TextColor
            };
            songRequests.RowTemplate.Height = 34;
            songRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            Song.FillWeight = 180;
            Author.FillWeight = 110;
            Viewer.FillWeight = 110;
            Id.FillWeight = 70;
            Id.HeaderText = GetLocalizedText("TwitchBot.Form.Id", "ID");

            content.Controls.Add(queueLabel);
            content.Controls.Add(EnableButton);
            content.Controls.Add(songRequests);

            ResumeLayout(true);
        }

        private static Font CreateUiFont(float size, FontStyle style)
        {
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
        }

        public static string GetLocalizedText(string key, string fallback)
        {
            return Properties.Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }

        private void AttachWindowDrag(Control control)
        {
            control.MouseDown += (sender, args) =>
            {
                if (args.Button != MouseButtons.Left) return;
                ReleaseCapture();
                SendMessage(Handle, WmNclButtonDown, new IntPtr(HtCaption), IntPtr.Zero);
            };
        }

        public Process RagnarockApp { get; set; }


        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://twitchtokengenerator.com/");
            Process.Start(sInfo);
        }

        public void checkEnabled()
        {
            if (!botEnabled)
            {
                _twitchBotEnabled = false;
                StopTwitchClient("disabled by user");
                return;
            }

            if (string.IsNullOrEmpty(_configuration.AuthTmi) || string.IsNullOrEmpty(_configuration.TwitchChannel))
            {
                MessageBox.Show(GetLocalizedText("TwitchBot.Message.ConfigureFirst", "Configure the Twitch bot in Preferences before enabling it."), "RagnaCustoms.com", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                botEnabled = false;
                _twitchBotEnabled = false;
                TwitchBotLogger.Warn("Bot start refused because Twitch credentials or channel are missing.");
                return;
            }

            if (_twitchClient != null && (_twitchClient.IsConnected || _connectionInProgress))
            {
                _twitchBotEnabled = true;
                TwitchBotLogger.Info("Start requested while a Twitch client is already active; reusing it.");
                return;
            }

            StopTwitchClient("replacing inactive client");

            try
            {
                var credentials = new ConnectionCredentials(_configuration.TwitchChannel, _configuration.AuthTmi);
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };

                var client = new TwitchClient(new WebSocketClient(clientOptions));
                client.Initialize(credentials, _configuration.TwitchChannel);
                client.OnLog += OnLog;
                client.OnMessageReceived += OnMessageReceived;
                client.OnUserBanned += OnUserBanned;
                client.OnConnected += OnConnected;
                client.OnConnectionError += OnConnectionError;
                client.OnDisconnected += OnDisconnected;
                client.OnError += OnError;

                _twitchClient = client;
                _twitchBotEnabled = true;
                _connectionInProgress = true;
                TwitchBotLogger.Info("Connecting Twitch client to channel '" + _configuration.TwitchChannel + "'.");
                client.Connect();
            }
            catch (Exception exception)
            {
                _connectionInProgress = false;
                _twitchBotEnabled = false;
                botEnabled = false;
                TwitchBotLogger.Error("Twitch client failed during startup.", exception);
                StopTwitchClient("startup failure");
            }
        }

        private void StopTwitchClient(string reason)
        {
            var client = _twitchClient;
            _twitchClient = null;
            _joinedChannel = null;
            _connectionInProgress = false;

            if (client == null) return;

            client.OnLog -= OnLog;
            client.OnMessageReceived -= OnMessageReceived;
            client.OnUserBanned -= OnUserBanned;
            client.OnConnected -= OnConnected;
            client.OnConnectionError -= OnConnectionError;
            client.OnDisconnected -= OnDisconnected;
            client.OnError -= OnError;

            try
            {
                if (client.IsConnected)
                    client.Disconnect();
                TwitchBotLogger.Info("Twitch client stopped (" + reason + ").");
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Twitch client failed while stopping (" + reason + ").", exception);
            }
        }

        private void OnLog(object sender, OnLogArgs e)
        {
            TwitchBotLogger.Info("TwitchLib: " + e.Data);
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            _connectionInProgress = false;
            TwitchBotLogger.Error("Twitch connection error: " + e.Error);
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            _connectionInProgress = false;
            TwitchBotLogger.Warn("Twitch client disconnected.");
        }

        private void OnError(object sender, OnErrorEventArgs e)
        {
            TwitchBotLogger.Error("Twitch communication error.", e.Exception);
        }

        private void OnUserBanned(object sender, OnUserBannedArgs e)
        {
            if (!ReferenceEquals(sender, _twitchClient)) return;
            TwitchBotLogger.Info("User banned: " + e.UserBan.Username);
            RemoveSongByRequester(e.UserBan.Username);
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            var client = sender as TwitchClient;
            if (client == null || !ReferenceEquals(client, _twitchClient))
            {
                TwitchBotLogger.Warn("Ignoring OnConnected from an obsolete Twitch client.");
                return;
            }

            _connectionInProgress = false;
            _joinedChannel = client.GetJoinedChannel(_configuration.TwitchChannel);
            TwitchBotLogger.Info("Twitch client connected to '" + e.AutoJoinChannel + "'.");

            if (!_configuration.DisableBotWelcome)
            {
                client.SendMessage(_joinedChannel, string.Format(GetLocalizedText("TwitchBot.Message.Connected", "{0} Ragnacustoms.com's bot connected"), _configuration.BotPrefix));
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
                    TwitchBotLogger.Error("Failed to load Twitch command '" + command.FullName + "'.", e);
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
            var client = sender as TwitchClient;
            if (!_twitchBotEnabled || client == null || !ReferenceEquals(client, _twitchClient)) return;

            try
            {
                var command = e.ChatMessage.Message.Split(' ');
                if (!_allowedPrefixes.Contains(command[0])) return;

                TwitchBotLogger.Info("Command received from '" + e.ChatMessage.Username + "': " + e.ChatMessage.Message);

                new Thread(() => ProcessMessage(client, command, e))
                {
                    IsBackground = true
                }.Start();
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Failed to receive a Twitch chat message.", exception);
            }
        }

        private void ProcessMessage(TwitchClient client, string[] command, OnMessageReceivedArgs e)
        {
            try
            {
                if (!_twitchBotEnabled || !ReferenceEquals(client, _twitchClient)) return;

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
                    var success = cmd.Action(_joinedChannel, client, this, e);
                    if (!success) client.SendMessage(_joinedChannel, string.Format(GetLocalizedText("TwitchBot.Message.Error", "{0}An error has occurred!"), _configuration.BotPrefix));
                }
                else
                {
                    var arg1 = command[1];
                    if (!Commandes.ContainsKey(arg1))
                    {
                        AddRequest(client, string.Join("%20", command.Skip(1).ToArray()), e);
                        return;
                    }

                    var cmd = Commandes[arg1];
                    if (cmd.IllegalUsers().Contains(e.ChatMessage.UserType))
                    {
                        client.SendMessage(_joinedChannel,
                            string.Format(GetLocalizedText("TwitchBot.Message.NotAllowed", "{0}You are not allowed to execute this command!"), _configuration.BotPrefix));
                        return;
                    }

                    var success = cmd.Action(_joinedChannel, client, this, e);
                    if (!success) client.SendMessage(_joinedChannel, string.Format(GetLocalizedText("TwitchBot.Message.Error", "{0}An error has occurred!"), _configuration.BotPrefix));
                }
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Unhandled exception while processing Twitch command '" + string.Join(" ", command) + "'.", exception);
            }
        }

        private void AddRequest(TwitchClient client, string requestId, OnMessageReceivedArgs e)
        {
            if (!QueueIsOpen)
            {
                client.SendMessage(_joinedChannel, string.Format(GetLocalizedText("TwitchBot.Message.QueueClosed", "{0}Queue is closed"), _configuration.BotPrefix));
                return;
            }

            var s = GetSongInfo(requestId) ?? SearshSong(requestId); // search song by id, if not found, search by name
            if (s != null)
            {
                client.SendMessage(_joinedChannel,
                    string.Format(GetLocalizedText("TwitchBot.Message.RequestInfo", "{0}Request info: {1}, mapped by {2}, requested by @{3}"), _configuration.BotPrefix, s.Name, s.Mapper, e.ChatMessage.Username));
                AddSongRequestToList(s, e.ChatMessage.Username);
                StartDownload(s.Id.ToString());
                //TwitchClient.SendMessage(joinedChannel, $"{prefixe}Ready: ");
            }
            else
            {
                client.SendMessage(_joinedChannel, string.Format(GetLocalizedText("TwitchBot.Message.SongNotFound", "{0}@{1} Song not found"), _configuration.BotPrefix, e.ChatMessage.Username));
            }
        }

        private void AddSongRequestToList(Song song, string viewer)
        {
            song.Requester = viewer;
            lock (_songListSync)
            {
                _songList.Add(song);
            }
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
            List<Song> songs;
            lock (_songListSync)
            {
                songs = _songList.FindAll(x => x.Requester == viewer);
                songs.ForEach(x => _songList.Remove(x));
            }

            if (songs.Count != 0)
            {
                songs.ForEach(x =>
                {
                    if (_configuration.EasyStreamRequest) removeSongEasyStream(x.Id);
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
            Song song;
            lock (_songListSync)
            {
                song = _songList.Find(s => string.Equals(s.Hash, hash, StringComparison.OrdinalIgnoreCase));
                if (song != null) _songList.Remove(song);
            }

            if (song == null) return;
            if (_configuration.EasyStreamRequest) removeSongEasyStream(song.Id);
            UpdateFormRows();
        }

        // set songRequest rows values to _songList values
        private void UpdateFormRows()
        {
            List<Song> songs;
            lock (_songListSync)
            {
                songs = _songList.ToList();
            }

            if (IsDisposed || !IsHandleCreated) return;

            try
            {
                songRequests.Invoke(new MethodInvoker(delegate
                {
                    songRequests.Rows.Clear();
                    foreach (var song in songs) songRequests.Rows.Add(song.Name, song.Author, song.Requester, song.Id);
                    songRequests.Refresh();
                }));
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Failed to update Twitch request queue UI.", exception);
            }
        }

        private Song GetSongInfo(string songId)
        {
            try
            {
                using var webClient = new WebClient();
                var json = webClient.DownloadString("https://api.ragnacustoms.com/api/song/" + songId);
                var stuff = JsonConvert.DeserializeObject<Song>(json);
                //debug_console.Items.Add($"Début de la récuperation de {stuff.title}");
                return stuff;
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Failed to fetch Twitch song information for '" + songId + "'.", exception);
                return null;
            }
        }

        private Song SearshSong(string search)
        {
            try
            {
                using var webClient = new WebClient();
                var json = webClient.DownloadString("https://api.ragnacustoms.com/api/search/" + search);
                var stuffs = JsonConvert.DeserializeObject<SearshResult>(json);
                return stuffs.FirstResultByName(search) ?? stuffs.BestResultByName(search);
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Failed to search Twitch song '" + search + "'.", exception);
                return null;
            }
        }




        private void TwitchBotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            botEnabled = false;
            _twitchBotEnabled = false;
            StopTwitchClient("form closed");
            _fileChangeEvent?.Dispose();

            lock (InstanceSync)
            {
                if (ReferenceEquals(_instance, this))
                    _instance = null;
            }

            TwitchBotLogger.Info("Twitch bot form closed.");
        }

        private void EnableButton_Click(object sender, EventArgs e)
        {
            botEnabled = !botEnabled;
            checkEnabled();
            EnableButton.Text = GetLocalizedText(botEnabled ? "TwitchBot.Form.Stop" : "TwitchBot.Form.Start", botEnabled ? "Stop" : "Start");
        }
    }
}
