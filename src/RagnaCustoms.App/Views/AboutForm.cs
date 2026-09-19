using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;

namespace RagnaCustoms.App.Views
{
    public partial class AboutForm : Form
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public AboutForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            SuspendLayout();

            BackColor = WindowColor;
            FormBorderStyle = FormBorderStyle.None;
            ClientSize = new Size(500, 250);
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
                Image = Resources.logocompressed,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            brand.Controls.Add(logo);

            var title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Window.About.Title", "RagnaCustoms  ·  About"),
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
                BackColor = SurfaceColor,
                Padding = new Padding(28, 24, 28, 24)
            };

            Controls.Clear();
            Controls.Add(content);
            Controls.Add(titleBar);

            label1.Text = GetLocalizedText("About.Form.Team", "DEVELOPMENT TEAM");
            label1.Location = new Point(28, 24);
            label1.ForeColor = AccentColor;
            label1.BackColor = Color.Transparent;
            label1.Font = CreateUiFont(9f, FontStyle.Bold);

            var links = new[] { linkLabel1, linkLabel2, linkLabel3, linkLabel4, linkLabel5 };
            for (var index = 0; index < links.Length; index++)
            {
                var link = links[index];
                link.AutoSize = true;
                link.BackColor = Color.Transparent;
                link.ForeColor = TextColor;
                link.LinkColor = AccentColor;
                link.ActiveLinkColor = Color.White;
                link.VisitedLinkColor = AccentColor;
                link.Font = CreateUiFont(10f, FontStyle.Regular);
                link.Location = index < 3
                    ? new Point(28 + index * 135, 70)
                    : new Point(28 + (index - 3) * 135, 112);
                content.Controls.Add(link);
            }

            content.Controls.Add(label1);
            ResumeLayout(true);
        }

        private static Font CreateUiFont(float size, FontStyle style)
        {
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
        }

        private static string GetLocalizedText(string key, string fallback)
        {
            return Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
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

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://github.com/pkshetlie");
            Process.Start(sInfo);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://github.com/dbraillon");
            Process.Start(sInfo);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://github.com/Crypt0-M3lon");
            Process.Start(sInfo);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://github.com/watsu78");
            Process.Start(sInfo);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://github.com/OcelusPRO");
            Process.Start(sInfo);
        }
    }
}
