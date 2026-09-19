using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;

namespace RagnaCustoms.App.Views
{
    public partial class AndroidDeviceSyncForm : Form
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public AndroidDeviceSyncForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private static string GetLocalizedText(string key, string fallback)
        {
            return Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }

        private void ApplyTheme()
        {
            SuspendLayout();
            BackColor = WindowColor;
            FormBorderStyle = FormBorderStyle.None;
            ClientSize = new Size(520, 190);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            Text = GetLocalizedText("Sync.Form.Title", "Oculus syncing");

            var titleBar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = WindowColor, Padding = new Padding(18, 0, 10, 0) };
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
                Text = Text,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = TextColor,
                Font = CreateUiFont(10f, FontStyle.Bold),
                Padding = new Padding(8, 0, 0, 0)
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

            var content = new Panel { Dock = DockStyle.Fill, BackColor = SurfaceColor, Padding = new Padding(28, 28, 28, 20) };
            Controls.Clear();
            Controls.Add(content);
            Controls.Add(titleBar);

            SyncingProgressBar.Location = new Point(28, 28);
            SyncingProgressBar.Size = new Size(464, 22);
            SyncingProgressBar.Style = ProgressBarStyle.Continuous;
            SyncingProgressBar.BackColor = InputColor;
            SyncingProgressBar.ForeColor = AccentColor;
            SyncingLabel.Location = new Point(28, 70);
            SyncingLabel.ForeColor = MutedTextColor;
            SyncingLabel.BackColor = Color.Transparent;
            SyncingLabel.Font = CreateUiFont(9f, FontStyle.Regular);
            content.Controls.Add(SyncingLabel);
            content.Controls.Add(SyncingProgressBar);
            ResumeLayout(true);
        }

        private static Font CreateUiFont(float size, FontStyle style)
        {
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
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

        private void DownloadingProgressBar_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
