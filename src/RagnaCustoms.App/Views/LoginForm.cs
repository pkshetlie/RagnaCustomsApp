using Newtonsoft.Json;
using RagnaCustoms.Models;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Communication.Interfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RagnaCustoms.App.Views
{
    public partial class LoginForm : Form
    {
        private SongForm songForm;
        private Configuration _configuration;
        private SongForm songView;

        public LoginForm(SongForm songView)
        {
            InitializeComponent();
            this.songView = songView;
            _configuration = new Configuration();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = inputUsername.Text;
            string password = inputPassword.Text;

            LoginAsync(username, password);
        }

        private async Task LoginAsync(string username, string password)
        {
        
            using var client = new HttpClient();

            var uri = new Uri($"https://api.ragnacustoms.com/api/login?username={username}&password={password}");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                _configuration.ApiKey = content;
                songView.changeLoginMenu();
                MessageBox.Show("Your API Key is now set", "RagnaCustoms.com", MessageBoxButtons.OK,
                     MessageBoxIcon.Information);

                new Preferences().Show();
                Dispose();
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();

                MessageBox.Show("Bad login : "+ content, "RagnaCustoms.com", MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://ragnacustoms.com/register");
            Process.Start(sInfo);
        
        }
    }
}
