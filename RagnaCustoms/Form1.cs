using Newtonsoft.Json;
using RagnaCustoms.Class;
using RagnaCustoms.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace RagnaCustoms
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            //Icon = new Icon("Logo.ico");
            InitializeComponent();
            Text = "RagnaCustoms.com";

            if (Program.Settings.AutoDownload)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.AutoDownload = checkBox1.Checked;
            Program.Settings.Save();
#if !DEBUG
            if (checkBox1.Checked)
            {
                Register.AddRegistryKeys();
            }
            else
            {
                Register.RemoveRegistryKeys();
            }
#endif
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var list = new List<Language>()
            {
                new Language() { Text = "Français", Code = "fr-FR" },
                new Language() { Text = "English", Code = "en-US" }
            };

            setTexts();

            checkBox1.Checked = Program.Settings.AutoDownload;
            checkBox2.Checked = Program.Settings.AutoClose;
            twitchChannel.Text = Program.Settings.TwitchChannel;
            twitchOAuth.Text = Program.Settings.AuthTmi;
            bot_enabled.Checked = Program.Settings.TwitchBotEnabled;
            comboBox1.DataSource = list;
            refreshApplication();

            comboBox1.ValueMember = "Code";
            comboBox1.DisplayMember = "Text";
            comboBox1.SelectedIndex = list.IndexOf(list.FirstOrDefault(x => x.Code == Program.Settings.Culture));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.twitch.tv/rhokapa/about");
            Process.Start(sInfo);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var code = ((Language)comboBox1.SelectedItem).Code;
            if (code != Program.Settings.Culture)
            {
                Program.Settings.Culture = code;
                Program.Settings.Save();
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Program.Settings.Culture);
                setTexts();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.AutoClose = checkBox2.Checked;
            Program.Settings.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var api = $"https://ragnacustoms.com/api/search/{textBox1.Text}";
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(api);
                var stuff = JsonConvert.DeserializeObject<SearchResponse>(json);

                comboBox3.DataSource = stuff.Results;
                comboBox3.ValueMember = "Id";
                comboBox3.DisplayMember = "fullTitle";
            }
        }

        private void setTexts()
        {
            checkBox1.Text = Resources.strings.Enable_OneClick_download_;
            checkBox2.Text = Resources.strings.Enable_auto_close_after_download_;
            searchLabel.Text = Resources.strings.Search;
            label1.Text = Resources.strings.Language;
            downloadButton.Text = Resources.strings.Download;
            helptwitchtmi.Text = Resources.strings.Get_your_Twitch_Chat_OAuth_Password_on;
            label2.Text = Resources.strings.Twitch_Chat_OAuth_password;
            label3.Text = Resources.strings.Your_Twitch_channel;
            bot_enabled.Text = Resources.strings.Enabled__;
            refreshApps.Text = Resources.strings.Refresh;
        }


        public void StartDownload(string song)
        {
            Process p = new Process();
            p.StartInfo.FileName = Application.ExecutablePath;
            p.StartInfo.Arguments = $"--install ragnac://install/{song}";
            p.Start();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedValue != null)
            {
                StartDownload(comboBox3.SelectedValue.ToString());
            }
            else
            {
                MessageBox.Show(Resources.strings.Error_1, "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitchapps.com/tmi/");
            Process.Start(sInfo);
        }

        private TwitchClient TwitchClient;
        private JoinedChannel joinedChannel;

        public Process RagnarockApp { get; set; }
        public TwitchPubSub client { get; private set; }

        private void bot_enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (bot_enabled.Checked && (String.IsNullOrEmpty(twitchOAuth.Text.ToString()) || String.IsNullOrEmpty(twitchChannel.Text)))
            {
                MessageBox.Show(Resources.strings.Error_2, "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bot_enabled.Checked = false;
                Program.Settings.TwitchBotEnabled = bot_enabled.Checked;
                Program.Settings.Save();
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

                client = new TwitchPubSub();

                client.ListenToRewards(twitchOAuth.Text);
                client.OnRewardRedeemed += OnRewardRedeemed;
                client.Connect();

                TwitchClient = new TwitchClient(new WebSocketClient(clientOptions));


                TwitchClient.Initialize(credentials, channel: twitchChannel.Text);

                TwitchClient.OnMessageReceived += OnMessageReceived;

                TwitchClient.OnConnected += OnConnected;
                TwitchClient.Connect();

            }
            Program.Settings.TwitchBotEnabled = bot_enabled.Checked;
            Program.Settings.Save();

        }

        private void OnRewardRedeemed(object sender, OnRewardRedeemedArgs e)
        {
            if (e != null)
            {

            }
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            joinedChannel = TwitchClient.GetJoinedChannel(twitchChannel.Text);
            TwitchClient.SendMessage(joinedChannel, Resources.strings.WelcomeBot);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {

            string[] command = e.ChatMessage.Message.Split(' ');
            if (e.ChatMessage.Message.Contains("RagnaCam"))
            {

            }
            switch (command[0])
            {
                case "!rc":
                    switch (command[1])
                    {
                        case "dev":
                            TwitchClient.SendMessage(joinedChannel, $"My dev is https://www.twitch.tv/rhokapa");
                            return;
                        case "cam":
                            //try
                            //{
                            int camera;
                            if (int.TryParse(command[2], out camera))
                            {
                                if (camera >= 1 && camera <= 5)
                                {
                                    ChangeCamera(command[2]);
                                    TwitchClient.SendMessage(joinedChannel, $"Camera changed");
                                }
                                else
                                {
                                    TwitchClient.SendMessage(joinedChannel, $"Camera error");
                                }
                            }
                            //}
                            //catch (Exception o_O)
                            //{
                            //    TwitchClient.SendMessage(joinedChannel, $"Camera error");
                            //}

                            return;
                        case "help":
                            TwitchClient.SendMessage(joinedChannel, $"commands : !rc cam <numero cam [1-5]> !rc dev (information about dev), !rc help (this command), !rc <song id> (download the map)");
                            return;
                    }
                    var s = GetSongInfo(command[1]);
                    if (s != null)
                    {
                        TwitchClient.SendMessage(joinedChannel, $"{Resources.strings.RequestInfo} : {s.fullTitle}, {Resources.strings.Mapped_by} : {s.Mapper}, asked by @{e.ChatMessage.Username}");
                        StartDownload(s.Id.ToString());
                        AddSongRequestToList(s, e.ChatMessage.Username);
                        TwitchClient.SendMessage(joinedChannel, $"Ready !");
                    }
                    else
                    {
                        TwitchClient.SendMessage(joinedChannel, $"@{e.ChatMessage.Username} {Resources.strings.Map_not_found}");
                    }
                    break;
            }
        }

        private void AddSongRequestToList(Song song, string viewer)
        {
            songRequests.Invoke(new MethodInvoker(delegate
             {
                 songRequests.Rows.Add(song.fullTitle, song.Author, viewer);
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


        private void twitchOAuth_TextChanged(object sender, EventArgs e)
        {
            bot_enabled.Checked = false;
            Program.Settings.AuthTmi = twitchOAuth.Text;
            Program.Settings.Save();

        }

        private void twitchChannel_TextChanged(object sender, EventArgs e)
        {
            bot_enabled.Checked = false;
            Program.Settings.TwitchChannel = twitchChannel.Text;
            Program.Settings.Save();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void refreshApplication()
        {
            RagnarockApp = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.Contains("Ragnarock-Win64"));
            if (RagnarockApp == null)
            {
                ragnarockApp.Text = Resources.strings.Ragnarock_application_not_found_please_refresh;
            }
            else
            {
                ragnarockApp.Text = Resources.strings.Ragnarock_application_found;
            }
        }

        private void refreshApps_Click(object sender, EventArgs e)
        {
            refreshApplication();
        }

        private void ChangeCamera(string num)
        {
            if (RagnarockApp != null)
            {
                ProcessHelper.SetFocusToExternalApp(RagnarockApp.ProcessName);

                FieldInfo info = typeof(SendKeys).GetField("keywords", BindingFlags.Static | BindingFlags.NonPublic);
                Thread.Sleep(1000);
                Array oldKeys = (Array)info.GetValue(null);
                Type elementType = oldKeys.GetType().GetElementType();
                Array newKeys = Array.CreateInstance(elementType, oldKeys.Length + 10);
                Array.Copy(oldKeys, newKeys, oldKeys.Length);
                for (int i = 0; i < 10; i++)
                {
                    var newItem = Activator.CreateInstance(elementType, "NUM" + i, (int)Keys.NumPad0 + i);
                    newKeys.SetValue(newItem, oldKeys.Length + i);
                }
                info.SetValue(null, newKeys);

                SendKeys.SendWait("{NUM" + num + "}");

            }
        }

        private void songRequests_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
