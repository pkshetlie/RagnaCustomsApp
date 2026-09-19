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
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using TwitchLib.Communication.Interfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RagnaCustoms.App.Views
{
    public partial class LoginForm : Form
    {
        private SongForm songForm;
        private Configuration _configuration;
        private SongForm songView;

        private static readonly Color BackgroundColor = Color.FromArgb(8, 19, 29);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 31, 43);
        private static readonly Color InputColor = Color.FromArgb(29, 45, 58);
        private static readonly Color BorderColor = Color.FromArgb(51, 70, 85);
        private static readonly Color TextColor = Color.FromArgb(239, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(137, 158, 174);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 216);

        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public LoginForm(SongForm songView)
        {
            InitializeComponent();
            this.songView = songView;
            _configuration = new Configuration();
            ApplyTheme();
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

            var uri = new Uri("https://api.ragnacustoms.com/api/login");
            using var payload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            var result = await client.PostAsync(uri, payload);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                _configuration.ApiKey = content;
                songView.changeLoginMenu();
                MessageBox.Show(GetLocalizedText("Login.Message.ApiKeySet", "Your API key is now set"), "RagnaCustoms.com", MessageBoxButtons.OK,
                     MessageBoxIcon.Information);

                new Preferences().Show();
                Dispose();
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();

                MessageBox.Show(string.Format(GetLocalizedText("Login.Message.BadLogin", "Login failed: {0}"), content), "RagnaCustoms.com", MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://ragnacustoms.com/register");
            Process.Start(sInfo);
        
        }

        private void ApplyTheme()
        {
            SuspendLayout();

            BackColor = BackgroundColor;
            ForeColor = TextColor;
            FormBorderStyle = FormBorderStyle.None;
            ClientSize = new Size(380, 430);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            Text = GetLocalizedText("Window.Login.Title", "RagnaCustoms · Login");
            AcceptButton = button1;

            Controls.Remove(Username);
            Controls.Remove(Password);
            Controls.Remove(button1);
            Controls.Remove(inputUsername);
            Controls.Remove(inputPassword);
            Controls.Remove(pictureBox1);
            Controls.Remove(linkLabel1);

            var titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = Color.FromArgb(13, 27, 39)
            };

            var title = new Label
            {
                AutoSize = true,
                Text = "RagnaCustoms",
                ForeColor = TextColor,
                Font = CreateUiFont(9.5f, FontStyle.Bold),
                Location = new Point(16, 12)
            };
            titleBar.Controls.Add(title);

            var closeButton = new Button
            {
                Text = "×",
                Size = new Size(44, 42),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(titleBar.ClientSize.Width - 44, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(13, 27, 39),
                ForeColor = TextColor,
                Font = CreateUiFont(16f, FontStyle.Regular),
                TabStop = false,
                UseVisualStyleBackColor = false
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (sender, args) => Close();
            closeButton.MouseEnter += (sender, args) => closeButton.BackColor = Color.FromArgb(150, 52, 68);
            closeButton.MouseLeave += (sender, args) => closeButton.BackColor = Color.FromArgb(13, 27, 39);
            titleBar.Controls.Add(closeButton);
            titleBar.Resize += (sender, args) => closeButton.Left = titleBar.ClientSize.Width - closeButton.Width;
            AttachWindowDrag(titleBar);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(54, 18, 54, 18)
            };

            pictureBox1.Location = new Point(135, 12);
            pictureBox1.Size = new Size(110, 64);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.Transparent;
            content.Controls.Add(pictureBox1);

            var heading = new Label
            {
                AutoSize = true,
                Text = GetLocalizedText("Login.Form.Title", "Login"),
                ForeColor = TextColor,
                Font = CreateUiFont(18f, FontStyle.Bold),
                Location = new Point(54, 86)
            };
            content.Controls.Add(heading);

            var subtitle = new Label
            {
                AutoSize = true,
                Text = GetLocalizedText("Login.Form.Subtitle", "Sign in to manage your songs."),
                ForeColor = MutedTextColor,
                Font = CreateUiFont(9f, FontStyle.Regular),
                Location = new Point(56, 119)
            };
            content.Controls.Add(subtitle);

            StyleLabel(Username, GetLocalizedText("Login.Form.Username", "Username"), new Point(54, 153));
            StyleLabel(Password, GetLocalizedText("Login.Form.Password", "Password"), new Point(54, 217));
            StyleInput(inputUsername, new Point(54, 174));
            StyleInput(inputPassword, new Point(54, 238));
            content.Controls.Add(Username);
            content.Controls.Add(inputUsername);
            content.Controls.Add(Password);
            content.Controls.Add(inputPassword);

            button1.Text = GetLocalizedText("Login.Form.Connect", "SIGN IN");
            button1.Location = new Point(54, 288);
            button1.Size = new Size(272, 40);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.BackColor = AccentColor;
            button1.ForeColor = Color.FromArgb(6, 24, 34);
            button1.Font = CreateUiFont(8.5f, FontStyle.Bold);
            button1.UseVisualStyleBackColor = false;
            content.Controls.Add(button1);

            linkLabel1.Text = GetLocalizedText("Login.Form.NoAccount", "No account? Create one");
            linkLabel1.LinkColor = AccentColor;
            linkLabel1.ActiveLinkColor = TextColor;
            linkLabel1.Font = CreateUiFont(8.5f, FontStyle.Regular);
            linkLabel1.Location = new Point(54, 345);
            content.Controls.Add(linkLabel1);

            Controls.Add(content);
            Controls.Add(titleBar);
            ResumeLayout(true);
        }

        private void StyleLabel(Label label, string text, Point location)
        {
            label.AutoSize = true;
            label.Text = text;
            label.ForeColor = MutedTextColor;
            label.Font = CreateUiFont(8.5f, FontStyle.Bold);
            label.Location = location;
        }

        private void StyleInput(TextBox input, Point location)
        {
            input.Location = location;
            input.Size = new Size(272, 28);
            input.AutoSize = false;
            input.BorderStyle = BorderStyle.FixedSingle;
            input.BackColor = InputColor;
            input.ForeColor = TextColor;
            input.Font = CreateUiFont(10f, FontStyle.Regular);
            input.Padding = new Padding(8, 3, 8, 0);
        }

        private static Font CreateUiFont(float size, FontStyle style)
        {
            return new Font("Bahnschrift", size, style, GraphicsUnit.Point);
        }

        private static string GetLocalizedText(string key, string fallback)
        {
            return Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }

        private void AttachWindowDrag(Control control)
        {
            control.MouseDown += WindowBar_MouseDown;
            foreach (Control child in control.Controls)
            {
                if (!(child is Button)) AttachWindowDrag(child);
            }
        }

        private void WindowBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WmNclButtonDown, new IntPtr(HtCaption), IntPtr.Zero);
        }
    }
}
