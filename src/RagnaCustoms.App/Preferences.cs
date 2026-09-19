using Microsoft.WindowsAPICodePack.Dialogs;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Configuration = RagnaCustoms.Services.Configuration;

namespace RagnaCustoms.App
{
    public partial class Preferences : Form
    {
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

        private Configuration _configuration;
        private Label songFolderPatternLabel;
        private TextBox songFolderPatternText;
        private Label songFolderPatternHelp;

        public Preferences()
        {
            InitializeComponent();
            _configuration = new Configuration();
            DefaultDirTxt.Text = _configuration.BaseFolder;
            textBox1.Text = _configuration.ApiKey;
            closeOnEndCheckbox.Checked = _configuration.AutoCloseDownload;
            RequestFolderText.Text = _configuration.RequestFolder;


            // OrderAlphabetCheckbox.Checked = _configuration.OrderAlphabetically;
            //OrderMapperCheckbox.Checked = _configuration.OrderMapper;

            copyRanked.Checked = _configuration.CopyRanked;
            twitchOAuth.Text = _configuration.AuthTmi;
            twitchChannel.Text = _configuration.TwitchChannel;
            autoStart.Checked = _configuration.TwitchBotAutoStart;
            prefix.Text = _configuration.BotPrefix;
            autoStart.Checked = _configuration.TwitchBotAutoStart;
            checkBox1.Checked = _configuration.DisableBotWelcome;

            radioButton1.Checked = !_configuration.OrderAlphabetically && !_configuration.OrderMapper;
            radioButton2.Checked = _configuration.OrderAlphabetically;
            radioButton3.Checked = _configuration.OrderMapper;

            ApplyTheme();
        }

        private void ApplyTheme()
        {
            SuspendLayout();

            BackColor = WindowColor;
            ForeColor = TextColor;
            FormBorderStyle = FormBorderStyle.None;
            ClientSize = new Size(900, 700);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;

            var titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = WindowColor,
                Padding = new Padding(18, 0, 10, 0)
            };

            var brandMark = new Panel
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
            brandMark.Controls.Add(logo);

            var title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Window.Preferences.Title", "RagnaCustoms  ·  Preferences"),
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
            titleBar.Controls.Add(brandMark);
            AttachWindowDrag(titleBar);
            AttachWindowDrag(brandMark);
            AttachWindowDrag(logo);
            AttachWindowDrag(title);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = WindowColor,
                Padding = new Padding(20, 18, 20, 20)
            };

            Controls.Clear();
            Controls.Add(content);
            Controls.Add(titleBar);

            ConfigureGroupBox(groupBox1, GetLocalizedText("Preferences.Form.General", "GENERAL"), new Point(20, 18), new Size(420, 220));
            ConfigureGroupBox(groupBox2, GetLocalizedText("Preferences.Form.Download", "DOWNLOAD"), new Point(20, 254), new Size(420, 78));
            ConfigureGroupBox(groupBox3, GetLocalizedText("Preferences.Form.Directories", "DIRECTORIES"), new Point(20, 348), new Size(420, 280));
            ConfigureGroupBox(groupBox4, GetLocalizedText("Preferences.Form.TwitchBot", "TWITCH BOT"), new Point(460, 18), new Size(420, 604));

            content.Controls.Add(groupBox1);
            content.Controls.Add(groupBox2);
            content.Controls.Add(groupBox3);
            content.Controls.Add(groupBox4);

            CreateSongFolderPatternControls();
            LayoutGeneralGroup();
            LayoutDownloadGroup();
            LayoutDirectoriesGroup();
            LayoutTwitchGroup();

            songFolderPatternText.Text = _configuration.SongFolderPattern;
            UpdateOrganizationOptionsAvailability();

            ResumeLayout(true);
        }

        private void ConfigureGroupBox(GroupBox groupBox, string title, Point location, Size size)
        {
            groupBox.Text = title;
            groupBox.Location = location;
            groupBox.Size = size;
            groupBox.BackColor = SurfaceColor;
            groupBox.ForeColor = AccentColor;
            groupBox.Font = CreateUiFont(9f, FontStyle.Bold);
            groupBox.FlatStyle = FlatStyle.Flat;
        }

        private void LayoutGeneralGroup()
        {
            label6.Text = GetLocalizedText("Preferences.Form.ScoreHelp", label6.Text);
            button1.Text = GetLocalizedText("Preferences.Form.ActivateScore", button1.Text).ToUpperInvariant();
            label2.Text = GetLocalizedText("Preferences.Form.ApiKey", label2.Text);
            defaultDirButton.Text = GetLocalizedText("Preferences.Form.Change", defaultDirButton.Text).ToUpperInvariant();
            label1.Text = GetLocalizedText("Preferences.Form.BaseDirectory", label1.Text);

            StyleLabel(label2, TextColor, 9f, FontStyle.Bold);
            label2.Location = new Point(16, 32);
            StyleInput(textBox1);
            textBox1.Location = new Point(16, 52);
            textBox1.Width = 388;

            StyleButton(button1, "ACTIVATE SCORE SENDING");
            button1.Location = new Point(16, 88);
            button1.Size = new Size(388, 34);

            StyleLabel(label6, MutedTextColor, 8f, FontStyle.Regular);
            label6.Location = new Point(16, 130);

            StyleLabel(label1, TextColor, 9f, FontStyle.Bold);
            label1.Location = new Point(16, 158);
            StyleInput(DefaultDirTxt);
            DefaultDirTxt.Location = new Point(16, 178);
            DefaultDirTxt.Width = 300;

            StyleButton(defaultDirButton, "CHANGE");
            defaultDirButton.Location = new Point(324, 178);
            defaultDirButton.Size = new Size(80, 28);
        }

        private void LayoutDownloadGroup()
        {
            closeOnEndCheckbox.Text = GetLocalizedText("Preferences.Form.CloseOnDownload", closeOnEndCheckbox.Text);
            StyleOption(closeOnEndCheckbox);
            closeOnEndCheckbox.Location = new Point(16, 31);
        }

        private void LayoutDirectoriesGroup()
        {
            copyRanked.Text = GetLocalizedText("Preferences.Form.CopyRanked", copyRanked.Text);
            label7.Text = GetLocalizedText("Preferences.Form.MapsOrganization", label7.Text);
            radioButton1.Text = GetLocalizedText("Preferences.Form.OneFolder", radioButton1.Text);
            radioButton2.Text = GetLocalizedText("Preferences.Form.Alphabetically", radioButton2.Text);
            radioButton3.Text = GetLocalizedText("Preferences.Form.ByMapper", radioButton3.Text);

            StyleOption(copyRanked);
            copyRanked.Location = new Point(16, 31);

            StyleLabel(label7, MutedTextColor, 8f, FontStyle.Bold);
            label7.Location = new Point(16, 66);

            StyleOption(radioButton1);
            radioButton1.Location = new Point(16, 88);
            StyleOption(radioButton2);
            radioButton2.Location = new Point(16, 118);
            StyleOption(radioButton3);
            radioButton3.Location = new Point(16, 148);

            songFolderPatternLabel.Text = GetLocalizedText("Preferences.Form.SongFolderPattern", "Nom du dossier des morceaux");
            StyleLabel(songFolderPatternLabel, TextColor, 9f, FontStyle.Bold);
            songFolderPatternLabel.Location = new Point(16, 178);

            StyleInput(songFolderPatternText);
            songFolderPatternText.Location = new Point(16, 198);
            songFolderPatternText.Width = 388;

            songFolderPatternHelp.Text = GetLocalizedText(
                "Preferences.Form.SongFolderPatternHelp",
                "Variables : $song.id · $song.name · $song.author · $mapper.name · $song.level · $date · $time\r\n$date.year · $date.month · $date.day");
            songFolderPatternHelp.Location = new Point(16, 232);
        }

        private void CreateSongFolderPatternControls()
        {
            songFolderPatternLabel = new Label();
            songFolderPatternText = new TextBox();
            songFolderPatternHelp = new Label
            {
                AutoSize = false,
                Width = 388,
                Height = 48,
                BackColor = Color.Transparent,
                ForeColor = MutedTextColor,
                Font = CreateUiFont(7.5f, FontStyle.Regular)
            };

            songFolderPatternText.TextChanged += SongFolderPatternText_TextChanged;
            groupBox3.Controls.Add(songFolderPatternLabel);
            groupBox3.Controls.Add(songFolderPatternText);
            groupBox3.Controls.Add(songFolderPatternHelp);
        }

        private void UpdateOrganizationOptionsAvailability()
        {
            var forceSingleFolder = SongFolderNameFormatter.StartsWithNumericToken(songFolderPatternText.Text);
            if (forceSingleFolder)
            {
                _configuration.OrderAlphabetically = false;
                _configuration.OrderMapper = false;
                radioButton1.Checked = true;
            }

            radioButton2.Visible = !forceSingleFolder;
            radioButton3.Visible = !forceSingleFolder;
            label7.Text = GetLocalizedText(
                forceSingleFolder ? "Preferences.Form.NumericPatternOrganization" : "Preferences.Form.MapsOrganization",
                forceSingleFolder ? "Organisation forcée : un seul dossier" : "Organisation des morceaux");
        }

        private void LayoutTwitchGroup()
        {
            button2.Text = GetLocalizedText("Preferences.Form.ClearRequests", button2.Text).ToUpperInvariant();
            checkBox1.Text = GetLocalizedText("Preferences.Form.DisableWelcome", checkBox1.Text);
            label5.Text = GetLocalizedText("Preferences.Form.RequestFolder", label5.Text);
            autoStart.Text = GetLocalizedText("Preferences.Form.AutoStart", autoStart.Text);
            label4.Text = GetLocalizedText("TwitchBot.Form.TMI", label4.Text);
            label3.Text = GetLocalizedText("Preferences.Form.TwitchChannel", label3.Text);
            botMessagePrefixLabel.Text = GetLocalizedText("Preferences.Form.BotPrefix", botMessagePrefixLabel.Text);
            helptwitchtmi.Text = GetLocalizedText("Preferences.Form.GetToken", helptwitchtmi.Text);

            StyleLabel(label4, TextColor, 9f, FontStyle.Bold);
            label4.Location = new Point(16, 32);
            StyleInput(twitchOAuth);
            twitchOAuth.Location = new Point(16, 52);
            twitchOAuth.Width = 388;

            StyleLabel(helptwitchtmi, MutedTextColor, 8f, FontStyle.Regular);
            helptwitchtmi.Location = new Point(16, 88);
            StyleLink(linkLabel2);
            linkLabel2.Location = new Point(104, 88);

            StyleLabel(label3, TextColor, 9f, FontStyle.Bold);
            label3.Location = new Point(16, 120);
            StyleInput(twitchChannel);
            twitchChannel.Location = new Point(16, 140);
            twitchChannel.Width = 388;

            StyleLabel(botMessagePrefixLabel, TextColor, 9f, FontStyle.Bold);
            botMessagePrefixLabel.Location = new Point(16, 180);
            StyleInput(prefix);
            prefix.Location = new Point(202, 176);
            prefix.Width = 70;

            StyleOption(autoStart);
            autoStart.Location = new Point(16, 216);
            StyleOption(checkBox1);
            checkBox1.Location = new Point(16, 246);

            StyleLabel(label5, TextColor, 9f, FontStyle.Bold);
            label5.Location = new Point(16, 284);
            StyleInput(RequestFolderText);
            RequestFolderText.Location = new Point(16, 304);
            RequestFolderText.Width = 388;

            StyleButton(button2, "CLEAR REQUESTS FOLDER");
            button2.Location = new Point(16, 350);
            button2.Size = new Size(388, 34);
        }

        private void StyleLabel(Label label, Color color, float size, FontStyle style)
        {
            label.AutoSize = true;
            label.ForeColor = color;
            label.BackColor = Color.Transparent;
            label.Font = CreateUiFont(size, style);
        }

        private void StyleInput(TextBox input)
        {
            input.AutoSize = false;
            input.Height = 28;
            input.BackColor = InputColor;
            input.ForeColor = TextColor;
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Font = CreateUiFont(9.5f, FontStyle.Regular);
            input.Padding = new Padding(8, 2, 8, 0);
        }

        private void StyleButton(Button button, string text)
        {
            button.Text = text;
            button.AutoSize = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = AccentColor;
            button.ForeColor = AccentTextColor;
            button.Font = CreateUiFont(8.5f, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }

        private void StyleOption(Control option)
        {
            option.BackColor = SurfaceColor;
            option.ForeColor = TextColor;
            option.Font = CreateUiFont(9f, FontStyle.Regular);
        }

        private void StyleLink(LinkLabel link)
        {
            link.AutoSize = true;
            link.BackColor = Color.Transparent;
            link.ForeColor = AccentColor;
            link.LinkColor = AccentColor;
            link.ActiveLinkColor = Color.White;
            link.VisitedLinkColor = AccentColor;
            link.Font = CreateUiFont(8f, FontStyle.Regular);
        }

        private static Font CreateUiFont(float size, FontStyle style)
        {
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
        }

        private static string GetLocalizedText(string key, string fallback)
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

        private void defaultDirButton_Click(object sender, EventArgs e)
        {

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = _configuration.BaseFolder;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _configuration.BaseFolder = dialog.FileName;
                DefaultDirTxt.Text = _configuration.BaseFolder;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _configuration.ApiKey = textBox1.Text;
        }

        private void closeOnEndCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            _configuration.AutoCloseDownload = closeOnEndCheckbox.Checked;
        }

        private void copyRanked_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.CopyRanked = copyRanked.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //_configuration.OrderAlphabetically = OrderAlphabetCheckbox.Checked;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void twitchOAuth_TextChanged(object sender, EventArgs e)
        {
            _configuration.AuthTmi = twitchOAuth.Text;
        }

        private void twitchChannel_TextChanged(object sender, EventArgs e)
        {
            _configuration.TwitchChannel = twitchChannel.Text;

        }

        private void RequestFolderText_TextChanged(object sender, EventArgs e)
        {
            _configuration.RequestFolder = RequestFolderText.Text;

        }

        private void SongFolderPatternText_TextChanged(object sender, EventArgs e)
        {
            _configuration.SongFolderPattern = songFolderPatternText.Text;
            UpdateOrganizationOptionsAvailability();
        }

        private void prefix_TextChanged(object sender, EventArgs e)
        {
            _configuration.BotPrefix = prefix.Text;
        }

        private void autoStart_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.TwitchBotAutoStart = autoStart.Checked;
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://twitchtokengenerator.com/");
            Process.Start(sInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Program.checkApiKey(_configuration.ApiKey))
            {


                var gameini = DirProvider.getGameIniFile().ToString();
                if (File.Exists(gameini))
                {
                    using (StreamWriter writer = new StreamWriter(gameini))
                    {
                        writer.WriteLine("[/Script/Ragnarock.RagnarockSettings]");
                        writer.WriteLine($"CustomApiURLs=\"https://api.ragnacustoms.com/wanapi/score/{_configuration.ApiKey}\"");
                    }
                }


                if (AndroidDevice.GetFirstFoundDevice() != null)
                {
                    AndroidDevice.GenerateGameIni();
                }

                MessageBox.Show(GetLocalizedText("Preferences.Message.GameIniUpdated", "File updated with this API key"), "RagnaCustoms.com", MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(GetLocalizedText("Preferences.Message.InvalidApiKey", "Your API key is not up to date. Please log in or set it manually before updating Game.ini."), "RagnaCustoms.com", MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation);

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OrderMapperCheckbox_CheckedChanged(object sender, EventArgs e)
        {
           // _configuration.OrderMapper = OrderMapperCheckbox.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.OrderAlphabetically = false;
            _configuration.OrderMapper = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.OrderAlphabetically = true;
            _configuration.OrderMapper = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.OrderAlphabetically = false;
            _configuration.OrderMapper = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void botMessagePrefixLabel_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo customDirectory = DirProvider.getCustomDirectory();
            var songDirectoryPath = Path.Combine(customDirectory.FullName, _configuration.RequestFolder);
          
            try
            {
                DeleteDirectoryContents(songDirectoryPath);

                MessageBox.Show(GetLocalizedText("Preferences.Message.RequestsCleared", "Map request folder cleared!"), "RagnaCustoms.com", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
            }
            catch (Exception o_O)
            {
                MessageBox.Show(o_O.Message, "RagnaCustoms.com", MessageBoxButtons.OK,
                                         MessageBoxIcon.Warning);
            }
        }

        private void DeleteDirectoryContents(string path)
        {
            try
            {
                // Delete files in the directory
                foreach (string filePath in Directory.GetFiles(path))
                {
                    File.Delete(filePath);
                }

                // Recursively delete subdirectories
                foreach (string subdirectoryPath in Directory.GetDirectories(path))
                {
                    DeleteDirectoryContents(subdirectoryPath);
                    Directory.Delete(subdirectoryPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting directory contents: {ex.Message}");
            }
        }

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
            _configuration.DisableBotWelcome = checkBox1.Checked;
        }
    }
}
