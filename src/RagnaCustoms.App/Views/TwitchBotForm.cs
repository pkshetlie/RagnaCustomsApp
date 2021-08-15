using Newtonsoft.Json;
using RagnaCustoms.Models;
using RagnaCustoms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace RagnaCustoms.App.Views
{
    public partial class TwitchBotForm : Form
    {
        private Configuration configuration;
        
        public TwitchBotForm() {
            InitializeComponent();
            configuration = new Configuration();

            twitchChannel.Text = configuration.TwitchChannel;
            twitchOAuth.Text = configuration.AuthTmi;
            loadCommands();
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitchapps.com/tmi/");
            Process.Start(sInfo);
        }

        private TwitchClient TwitchClient;
        private JoinedChannel joinedChannel;
        private bool TwitchBotEnabled = false;
        public Process RagnarockApp { get; set; }
        
        private void bot_enabled_CheckedChanged_1(object sender, EventArgs e)
        {
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

                TwitchClient = new TwitchClient(new WebSocketClient(clientOptions));

                TwitchClient.Initialize(credentials, channel: twitchChannel.Text);
                TwitchClient.OnMessageReceived += OnMessageReceived;

                TwitchClient.OnConnected += OnConnected;
                TwitchClient.Connect();

            }
            TwitchBotEnabled = bot_enabled.Checked;
        }
        
        public string prefixe { get; set; }
        private void OnConnected(object sender, OnConnectedArgs e) {
            joinedChannel = TwitchClient.GetJoinedChannel(twitchChannel.Text);
            TwitchClient.SendMessage(joinedChannel, $"{prefixe} Ragnacustoms.com's bot connected");
            //TwitchClient.SendMessage(joinedChannel, $"{prefixe}{Resources.app.strings.WelcomeBot}");
        }
        
        public bool QueueIsOpen = true;

        public Dictionary<string, ICommandes> commandes = new Dictionary<string, ICommandes>();

        private void loadCommands()
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
                        foreach (var name in cmd.names())
                        {
                            commandes.Add(name, cmd);
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

        private void addRequest(string requestId, OnMessageReceivedArgs e)
        {
            if (!QueueIsOpen)
            {
                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Queue is closed");
                return;
            }

            var s = GetSongInfo(requestId);
            if (s != null)
            {
                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Request Info: {s.Name}, Mapped by : {s.Mapper}, asked by @{e.ChatMessage.Username}");
                StartDownload(s.Id.ToString());
                AddSongRequestToList(s, e.ChatMessage.Username);
                //TwitchClient.SendMessage(joinedChannel, $"{prefixe}Ready: ");
            }
            else
            {
                TwitchClient.SendMessage(joinedChannel, $"{prefixe}@{e.ChatMessage.Username} Song not found");
            }
        }
        
        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!TwitchBotEnabled) { return; }

            string[] command = e.ChatMessage.Message.Split(' ');

            if (!_allowedPrefixes.Contains(command[0]))
            {
                return;
            }

            if (command.Length < 2)
            {
                if (!commandes.ContainsKey(""))
                {
                    return;
                }
                var cmd = commandes[""];
                var success = cmd.action(joinedChannel, prefix, TwitchClient,this,  e);
                if (!success)
                {
                    TwitchClient.SendMessage(joinedChannel, $"{prefixe}An error has occurred !");
                }
            }
            else
            {
                new Thread(() => 
                {
                    Thread.CurrentThread.IsBackground = true; 
                    var arg1 = command[1];
                    if (!commandes.ContainsKey(arg1))
                    {
                        addRequest(arg1, e);
                        return;
                    }
                    var cmd = commandes[arg1];
                    var success = cmd.action(joinedChannel, prefix, TwitchClient, this, e);
                    if (!success)
                    {
                        TwitchClient.SendMessage(joinedChannel, $"{prefixe}An error has occurred !");
                    }
                }).Start();
            }
        }

        private void StartDownload(string v) 
        {
            ProcessStartInfo sInfo = new ProcessStartInfo($"ragnac://install/{v}");
            Process.Start(sInfo);
        }

        private void AddSongRequestToList(Song song, string viewer) 
        {
            songRequests.Invoke(new MethodInvoker(delegate {
                songRequests.Rows.Add(song.Name, song.Author, viewer, song.Id);
            } ));
        }
        public void RemoveAtSongRequestInList(int i) 
        {
            songRequests.Invoke(new MethodInvoker(delegate {
                songRequests.Rows.RemoveAt(i);
            } ));
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
            catch (WebException o_O) 
            {
                return null;
            }
        }
        
        private void twitchOAuth_TextChanged_1(object sender, EventArgs e) 
        {
            bot_enabled.Checked = false;
            configuration.AuthTmi = twitchOAuth.Text;
        }

        private void twitchChannel_TextChanged(object sender, EventArgs e) {
            bot_enabled.Checked = false;
            configuration.TwitchChannel = twitchChannel.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private void helptwitchtmi_Click(object sender, EventArgs e) { }

        private void botMessagePrefixLabel_Click(object sender, EventArgs e) { }

        private void TwitchBotForm_FormClosed(object sender, FormClosedEventArgs e) 
        {
            bot_enabled.Checked = false;
            TwitchBotEnabled = false;
            if (TwitchClient.IsConnected) 
                TwitchClient.Disconnect();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) 
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitch.tv/rhokapa");
            Process.Start(sInfo);
        }
    }
}
