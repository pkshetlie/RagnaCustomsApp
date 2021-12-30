using Newtonsoft.Json;
using RagnaCustoms.Models;
using RagnaCustoms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.Presenters;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace RagnaCustoms.App.Views
{
    public partial class TwitchBotForm : Form
    {
        public Configuration _configuration;
        public TwitchBotForm() {
            InitializeComponent();
            
            _configuration = new Configuration();
            twitchChannel.Text = _configuration.TwitchChannel;
            twitchOAuth.Text = _configuration.AuthTmi;
            autoStart.Checked = _configuration.TwitchBotAutoStart;
            checkBox_autoLaunchTwitchBot.Checked = _configuration.TwitchBotAutoLaunch;
            Checkbox_EasyStreamRequest.Checked = _configuration.EasyStreamRequest;
            bot_enabled.Checked = _configuration.TwitchBotAutoStart;

            LoadCommands();
            
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitchapps.com/tmi/");
            Process.Start(sInfo);
        }

        private TwitchClient _twitchClient;
        private JoinedChannel _joinedChannel;
        private bool _twitchBotEnabled = false;
        public Process RagnarockApp { get; set; }

        private void bot_enabled_CheckedChanged_1(object sender, EventArgs e)
        {
            checkEnabled();
        }
        public void checkEnabled() { 
            if (bot_enabled.Checked && (String.IsNullOrEmpty(twitchOAuth.Text.ToString()) || String.IsNullOrEmpty(twitchChannel.Text))) {
                MessageBox.Show("You need to configure before enable", "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show(Resources.app.strings.Error_2, "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bot_enabled.Checked = false;
                return;
            }
            if (bot_enabled.Checked) {
                var credentials = new ConnectionCredentials(twitchChannel.Text, twitchOAuth.Text);
                var clientOptions = new ClientOptions {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };               

                _twitchClient = new TwitchClient(new WebSocketClient(clientOptions));

                _twitchClient.Initialize(credentials, channel: twitchChannel.Text);
                _twitchClient.OnMessageReceived += OnMessageReceived;

                _twitchClient.OnConnected += OnConnected;
                _twitchClient.Connect();

            }
            _twitchBotEnabled = bot_enabled.Checked;
        }
        
        public string Prefixe { get; set; }
        private void OnConnected(object sender, OnConnectedArgs e) {
            _joinedChannel = _twitchClient.GetJoinedChannel(twitchChannel.Text);
            _twitchClient.SendMessage(_joinedChannel, $"{Prefixe} Ragnacustoms.com's bot connected");
            //TwitchClient.SendMessage(joinedChannel, $"{prefixe}{Resources.app.strings.WelcomeBot}");
        }
        
        public bool QueueIsOpen = true;

        public Dictionary<string, ICommandes> Commandes = new Dictionary<string, ICommandes>();

        private void LoadCommands()
        {
            var type = typeof(ICommandes);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (Type command in types)
            {
                try
                {
                    var cmd = Activator.CreateInstance(command) as ICommandes;
                    if (cmd != null)
                    {
                        foreach (var name in cmd.Names())
                        {
                            Commandes.Add(name, cmd);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private readonly List<string> _allowedPrefixes = new List<string>
        {
            "!rc","!rcs", "!rcr"
        };

        private void AddRequest(string requestId, OnMessageReceivedArgs e)
        {
            if (!QueueIsOpen)
            {
                _twitchClient.SendMessage(_joinedChannel, $"{Prefixe}Queue is closed");
                return;
            }

            var s = GetSongInfo(requestId) ?? SearshSong(requestId); // search song by id, if not found, search by name
            if (s != null)
            {
                _twitchClient.SendMessage(_joinedChannel, $"{Prefixe}Request Info: {s.Name}, Mapped by : {s.Mapper}, asked by @{e.ChatMessage.Username}");
                AddSongRequestToList(s, e.ChatMessage.Username);
                StartDownload(s.Id.ToString());
                //TwitchClient.SendMessage(joinedChannel, $"{prefixe}Ready: ");
            }
            else
            {
                _twitchClient.SendMessage(_joinedChannel, $"{Prefixe}@{e.ChatMessage.Username} Song not found");
            }
        }
        
        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!_twitchBotEnabled) { return; }

            string[] command = e.ChatMessage.Message.Split(' ');

            if (!_allowedPrefixes.Contains(command[0]))
            {
                return;
            }
            new Thread((() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (_configuration.ViewerLang != null && _configuration.ViewerLang.ContainsKey(e.ChatMessage.UserId))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(_configuration.ViewerLang[e.ChatMessage.UserId], true);
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                }
                
                if (command.Length < 2)
                {
                    if (!Commandes.ContainsKey(""))
                    {
                        return;
                    }
                    var cmd = Commandes[""];
                    var success = cmd.Action(_joinedChannel, prefix, _twitchClient,this,  e);
                    if (!success)
                    {
                        _twitchClient.SendMessage(_joinedChannel,$"{Prefixe}An error has occurred !");
                    }
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
                        _twitchClient.SendMessage(_joinedChannel, $"{Prefixe}Vous n'avez pas l'autorisation d'éxécuter cette commande !");
                        return;
                    }
                    var success = cmd.Action(_joinedChannel, prefix, _twitchClient, this, e);
                    if (!success)
                    {
                        _twitchClient.SendMessage(_joinedChannel, $"{Prefixe}An error has occurred !");
                    }
                }
            })).Start();
        }

        private void StartDownload(string v) 
        {
            ProcessStartInfo sInfo = new ProcessStartInfo($"ragnac://install/{v}");
            Process.Start(sInfo);
        }

        private void AddSongRequestToList(Song song, string viewer) 
        {
            if (songRequests.Rows.Count <= 1 && _configuration.EasyStreamRequest)
            {
                DirProvider.getCustomDirectory().MoveTo(DirProvider.RagnarockBackupSongDirectoryPath);
                // rename custom songs directory and create new one
            }
            songRequests.Invoke(new MethodInvoker(delegate {
                songRequests.Rows.Add(song.Name, song.Author, viewer, song.Id);
            } ));
        }
        public void RemoveAtSongRequestInList(int i) 
        {
            songRequests.Invoke(new MethodInvoker(delegate {
                songRequests.Rows.RemoveAt(i);
            } ));
            
            if (songRequests.Rows.Count <= 1 && _configuration.EasyStreamRequest)
            {
                DirProvider.getCustomDirectory().Delete(true);
                DirProvider.getCustomBackupDirectory().MoveTo(DirProvider.RagnarockSongDirectoryPath);
            }
        }


        private Song GetSongInfo(string songId) 
        {
            try 
            {
                using var webClient = new System.Net.WebClient();
                var json = webClient.DownloadString("https://ragnacustoms.com/api/song/" + songId);
                Song stuff = JsonConvert.DeserializeObject<Song>(json);
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
                using var webClient = new System.Net.WebClient();
                var json = webClient.DownloadString("https://ragnacustoms.com/api/search/" + search);
                SearshResult stuffs = JsonConvert.DeserializeObject<SearshResult>(json);
                return stuffs.FirstResultByName(search) ?? stuffs.BestResultByName(search);
            }
            catch (WebException) 
            {
                return null;
            }
        }
        
        private void twitchOAuth_TextChanged_1(object sender, EventArgs e) 
        {
            bot_enabled.Checked = false;
            _configuration.AuthTmi = twitchOAuth.Text;
        }

        private void twitchChannel_TextChanged(object sender, EventArgs e) {
            bot_enabled.Checked = false;
            _configuration.TwitchChannel = twitchChannel.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private void helptwitchtmi_Click(object sender, EventArgs e) { }

        private void botMessagePrefixLabel_Click(object sender, EventArgs e) { }

        private void TwitchBotForm_FormClosed(object sender, FormClosedEventArgs e) 
        {
            if (bot_enabled.Checked)
            {
                bot_enabled.Checked = false;
                _twitchBotEnabled = false;
                if (_twitchClient.IsConnected) 
                    _twitchClient.Disconnect();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) 
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.twitch.tv/ragnacustoms_com");
            Process.Start(sInfo);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.TwitchBotAutoStart = autoStart.Checked;
        }

        private void prefix_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            _configuration.EasyStreamRequest = Checkbox_EasyStreamRequest.Checked;
        }

        private void checkBox_autoStartTwitchBot_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.TwitchBotAutoLaunch = checkBox_autoLaunchTwitchBot.Checked;
        }
    }
}
