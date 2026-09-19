using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.Presenters;

namespace RagnaCustoms.Views
{
    public partial class DownloadingForm : Form, IDownloadingView
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);
        private Label _windowTitleLabel;

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public DownloadingForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private static string GetLocalizedText(string key, string fallback)
        {
            return Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }

        public virtual DownloadingPresenter Presenter { private get; set; }

        public virtual int DownloadPercent
        {
            get => DownloadingProgressBar.Value;
            set => DownloadingProgressBar.Value = value;
        }

        public string Title
        {
            get => Text;
            set
            {
                Text = value;
                if (_windowTitleLabel != null) _windowTitleLabel.Text = value;
            }
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
            Text = GetLocalizedText("Downloading.Form.Title", "RagnaCustoms");

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
            _windowTitleLabel = new Label
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
            titleBar.Controls.Add(_windowTitleLabel);
            titleBar.Controls.Add(closeButton);
            titleBar.Controls.Add(brand);
            AttachWindowDrag(titleBar);
            AttachWindowDrag(brand);
            AttachWindowDrag(logo);
            AttachWindowDrag(_windowTitleLabel);

            var content = new Panel { Dock = DockStyle.Fill, BackColor = SurfaceColor, Padding = new Padding(28, 28, 28, 20) };
            Controls.Clear();
            Controls.Add(content);
            Controls.Add(titleBar);

            DownloadingProgressBar.Location = new Point(28, 28);
            DownloadingProgressBar.Size = new Size(464, 22);
            DownloadingProgressBar.Style = ProgressBarStyle.Continuous;
            DownloadingProgressBar.BackColor = InputColor;
            DownloadingProgressBar.ForeColor = AccentColor;
            DownloadingLabel.Text = GetLocalizedText("Downloading.Form.Message", DownloadingLabel.Text);
            DownloadingLabel.Location = new Point(28, 70);
            DownloadingLabel.ForeColor = MutedTextColor;
            DownloadingLabel.BackColor = Color.Transparent;
            DownloadingLabel.Font = CreateUiFont(9f, FontStyle.Regular);
            content.Controls.Add(DownloadingLabel);
            content.Controls.Add(DownloadingProgressBar);
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

        public virtual void ShowAsPopup()
        {
            ShowDialog();
        }

        public virtual void ShowSuccessMessage(string message, string title)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void DownloadingForm_Load(object sender, EventArgs e)
        {
        }
    }
}
