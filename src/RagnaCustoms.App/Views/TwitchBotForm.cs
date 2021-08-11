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
        public TwitchBotForm()
        {
            InitializeComponent();
            configuration = new Configuration();

            twitchChannel.Text = configuration.TwitchChannel;
            twitchOAuth.Text = configuration.AuthTmi;

        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitchapps.com/tmi/");
            Process.Start(sInfo);
        }

        private TwitchClient TwitchClient;
        private JoinedChannel joinedChannel;
        private bool TwitchBotEnabled = false;
        public Process RagnarockApp { get; set; }
        
        private void bot_enabled_CheckedChanged_1(object sender, EventArgs e)
        {
            if (bot_enabled.Checked && (String.IsNullOrEmpty(twitchOAuth.Text.ToString()) || String.IsNullOrEmpty(twitchChannel.Text)))
            {
                MessageBox.Show("You need to configure before enable", "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show(Resources.app.strings.Error_2, "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bot_enabled.Checked = false;
                return;
            }
            if (bot_enabled.Checked)
            {

                var credentials = new ConnectionCredentials(twitchChannel.Text, twitchOAuth.Text);
                var clientOptions = new ClientOptions
                {
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
        private void OnConnected(object sender, OnConnectedArgs e)
        {
            joinedChannel = TwitchClient.GetJoinedChannel(twitchChannel.Text);
            TwitchClient.SendMessage(joinedChannel, $"{prefixe} Ragnacustoms.com's bot connected");
            //TwitchClient.SendMessage(joinedChannel, $"{prefixe}{Resources.app.strings.WelcomeBot}");
        }
        public bool QueueIsOpen = true;

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!TwitchBotEnabled) { return; }

            string[] command = e.ChatMessage.Message.Split(' ');

            var part1 = command[0];
            if (part1 != "!rc")
            {
                return;
            }

            if (command.Length < 2)
            {
                TwitchClient.SendMessage(joinedChannel, $"{prefixe} Error on command");
                return;
            }

            var part2 = command[1];

            switch (part1)
            {
                case "!rc":
                    #region specific commands
                    switch (part2)
                    {
                        case "open":
                            if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Viewer)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Sorry only moderator can do that");
                                return;
                            }
                            if (!QueueIsOpen)
                            {
                                QueueIsOpen = true;
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Queue is now open");
                            }
                            else
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Queue is already open");
                            }
                            return;
                        case "close":
                            if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Viewer)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Sorry only moderator can do that");
                                return;
                            }
                            if (!QueueIsOpen)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Queue is already closed");
                            }
                            else
                            {
                                QueueIsOpen = false;
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Queue is now closed");
                            }
                            return;
                        case "shift":
                        case "done":
                            if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Viewer)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Sorry only moderator can do that.");
                                return;
                            }
                            try
                            {
                                RemoveAtSongRequestInList(0);
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Song removed");

                                var sog = songRequests.Rows[0].Cells["Song"].Value.ToString();
                                if (sog != null)
                                {
                                    TwitchClient.SendMessage(joinedChannel, $"{prefixe}Next song : {sog} ");
                                }
                                else
                                {
                                    TwitchClient.SendMessage(joinedChannel, $"{prefixe}End of the queue");
                                }
                            }
                            catch (Exception O_o)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}No More song to remove");
                            }
                            return;
                        case "next":
                            var song = songRequests.Rows[0].Cells["Song"].Value.ToString();
                            if (song != null)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}Next song : {song} ");
                            }
                            else
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}End of the queue");
                            }
                            return;

                        case "queue":
                            var songs = new List<string>();
                            var counter = 0;
                            var concatStr = "";
                            for (var i = 0; songRequests.Rows.Count - 1 > i; i++)
                            {
                                var songStr = songRequests.Rows[i].Cells["Song"].Value.ToString();
                                if (concatStr.Length + songStr.Length > 500)
                                {
                                    songs.Add(concatStr);
                                    concatStr = songStr;
                                    counter = songStr.Length;
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(concatStr))
                                    {
                                        concatStr += " // ";
                                    }
                                    concatStr += songStr;
                                }
                            }
                            songs.Add(concatStr);

                            TwitchClient.SendMessage(joinedChannel, $"{prefixe}Next songs are: ");
                            Thread.Sleep(400);
                            foreach (var songMessage in songs)
                            {
                                TwitchClient.SendMessage(joinedChannel, $"{prefixe}{songMessage}");
                            }
                            return;
                        case "cancel":

                            for (var i = songRequests.Rows.Count - 2; 0 <= i; i--)
                            {
                                if (songRequests.Rows[i].Cells["Viewer"].Value.ToString() == e.ChatMessage.Username)
                                {
                                    var sng = songRequests.Rows[i].Cells["Song"].Value.ToString();
                                    try
                                    {
                                        RemoveAtSongRequestInList(i);
                                        TwitchClient.SendMessage(joinedChannel, $"{prefixe}Song Cancelled: {sng} ");
                                    }
                                    catch (Exception o_O)
                                    {
                                        TwitchClient.SendMessage(joinedChannel, $"{prefixe}No More song to remove");
                                    }
                                    return;
                                }
                            }
                            return;
                        case "version":
                        case "v":
                            TwitchClient.SendMessage(joinedChannel, $"{prefixe}I'm version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
                            return;

                       
                        case "how-to":
                        case "ht":
                            TwitchClient.SendMessage(joinedChannel, $"{prefixe}Go to https://ragnacustoms.com, click on the twitch button to copy !rc &lt;songid&gt; and paste it here");
                            return;
                        case "help":
                            TwitchClient.SendMessage(joinedChannel, $"{prefixe}Help 1/2 : !rc help (this command), !rc &lt;song id&gt; (download the map), !rc cancel (remove last song you request)");
                            TwitchClient.SendMessage(joinedChannel, $"{prefixe}Help 2/2 : !rc open (open queue), !rc close (close queue), !rc shift (remove first song in list), !rc queue (list of songs not played), !rc next (next song to play), !rc version (to know current version)");
                            return;
                    }
                    #endregion
                    if (!QueueIsOpen)
                    {
                        TwitchClient.SendMessage(joinedChannel, $"{prefixe}Queue is closed");
                        return;
                    }

                    var s = GetSongInfo(part2);
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
                    break;
            }
        }

        private void StartDownload(string v)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo($"ragnac://install/{v}");
            Process.Start(sInfo);
        }

        private void AddSongRequestToList(Song song, string viewer)
        {
            songRequests.Invoke(new MethodInvoker(delegate
            {
                songRequests.Rows.Add(song.Name, song.Author, viewer, song.Id);
            }
             ));
        }
        private void RemoveAtSongRequestInList(int i)
        {
            songRequests.Invoke(new MethodInvoker(delegate
            {
                songRequests.Rows.RemoveAt(i);
            }
             ));
        }


        private Song GetSongInfo(string songId)
        {
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString("https://ragnacustoms.com/api/song/" + songId);
                    Song stuff = JsonConvert.DeserializeObject<Song>(json);
                    //debug_console.Items.Add($"Début de la récuperation de {stuff.title}");
                    return stuff;
                }
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

        private void twitchChannel_TextChanged(object sender, EventArgs e)
        {
            bot_enabled.Checked = false;
            configuration.TwitchChannel = twitchChannel.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void helptwitchtmi_Click(object sender, EventArgs e)
        {

        }

        private void botMessagePrefixLabel_Click(object sender, EventArgs e)
        {

        }

        private void TwitchBotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            bot_enabled.Checked = false;
            TwitchBotEnabled = false;
            if (TwitchClient.IsConnected)
            {
                TwitchClient.Disconnect();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitch.tv/rhokapa");
            Process.Start(sInfo);
        }
    }
}
