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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Configuration = RagnaCustoms.Services.Configuration;

namespace RagnaCustoms.App
{
    public partial class Preferences : Form
    {
        private Configuration _configuration;

        public Preferences()
        {
            InitializeComponent();
            _configuration = new Configuration();
            DefaultDirTxt.Text = _configuration.BaseFolder;
            textBox1.Text = _configuration.ApiKey;
            closeOnEndCheckbox.Checked = _configuration.AutoCloseDownload;
            OrderAlphabetCheckbox.Checked = _configuration.OrderAlphabetically;
            copyRanked.Checked = _configuration.CopyRanked;
            twitchOAuth.Text = _configuration.AuthTmi;
            twitchChannel.Text = _configuration.TwitchChannel;
            autoStart.Checked = _configuration.TwitchBotAutoStart;
            RequestFolderText.Text = _configuration.RequestFolder;
            prefix.Text = _configuration.BotPrefix;
            autoStart.Checked = _configuration.TwitchBotAutoStart;

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
            _configuration.OrderAlphabetically = OrderAlphabetCheckbox.Checked;
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
            var sInfo = new ProcessStartInfo("https://twitchapps.com/tmi/");
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

                MessageBox.Show("File updated with this API Key", "RagnaCustoms.com", MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Your API key is not up to date, please login or set it manualy before setting Game.ini", "RagnaCustoms.com", MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation);

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
