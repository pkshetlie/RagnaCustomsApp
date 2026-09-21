using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;

namespace RagnaCustoms.App.Views
{
    public enum PremiumSearchKind
    {
        Playlists,
        Artists
    }

    public sealed class PremiumSearchForm : Form
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color ElevatedColor = Color.FromArgb(24, 43, 58);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        private readonly PremiumSearchKind _kind;
        private TextBox _searchTextBox;
        private Button _searchButton;
        private DataGridView _resultsGrid;
        private Panel _premiumNotice;
        private Label _emptyState;
        private bool _hasPremiumAccess;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public PremiumSearchForm(PremiumSearchKind kind)
        {
            _kind = kind;
            _hasPremiumAccess = false;
            BuildForm();
        }

        public bool HasPremiumAccess
        {
            get { return _hasPremiumAccess; }
        }

        public void SetPremiumAccess(bool hasPremiumAccess)
        {
            _hasPremiumAccess = hasPremiumAccess;
            _premiumNotice.Visible = !hasPremiumAccess;
            _searchTextBox.Enabled = hasPremiumAccess;
            _searchButton.Enabled = hasPremiumAccess;
            _resultsGrid.Enabled = hasPremiumAccess;
            _emptyState.Text = hasPremiumAccess
                ? GetLocalizedText("Premium.Form.Empty", "Enter a search to see the results.")
                : GetLocalizedText("Premium.Form.ApiPending", "Premium verification and search will be available when the API is connected.");
        }

        private void BuildForm()
        {
            SuspendLayout();

            Text = GetLocalizedText(
                _kind == PremiumSearchKind.Playlists
                    ? "Premium.Form.Playlists.Title"
                    : "Premium.Form.Artists.Title",
                _kind == PremiumSearchKind.Playlists
                    ? "RagnaCustoms  ·  Playlist search"
                    : "RagnaCustoms  ·  Artist search");
            BackColor = WindowColor;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ClientSize = new Size(920, 620);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;

            var titleBar = BuildTitleBar();
            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(30, 26, 30, 28)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 5
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            var heading = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Form.Heading", "PREMIUM FEATURE"),
                ForeColor = AccentColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var description = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText(
                    _kind == PremiumSearchKind.Playlists
                        ? "Premium.Form.Playlists.Description"
                        : "Premium.Form.Artists.Description",
                    _kind == PremiumSearchKind.Playlists
                        ? "Find and download public playlists."
                        : "Find songs by artist, even when names are written differently."),
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(13f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0, 12, 0, 10)
            };
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 124));

            _searchTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = InputColor,
                ForeColor = TextColor,
                Font = CreateUiFont(10f, FontStyle.Regular),
                Margin = new Padding(0, 0, 8, 0),
                Enabled = false
            };
            _searchTextBox.KeyDown += SearchTextBox_KeyDown;

            _searchButton = new Button
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Form.Search", "SEARCH").ToUpperInvariant(),
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentColor,
                ForeColor = Color.FromArgb(6, 24, 34),
                Font = CreateUiFont(8.5f, FontStyle.Bold),
                Enabled = false,
                Margin = new Padding(0)
            };
            _searchButton.FlatAppearance.BorderSize = 0;
            _searchButton.Click += SearchButton_Click;
            searchLayout.Controls.Add(_searchTextBox, 0, 0);
            searchLayout.Controls.Add(_searchButton, 1, 0);

            var resultsPanel = BuildResultsPanel();
            _premiumNotice = BuildPremiumNotice();
            _emptyState = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Form.ApiPending", "Premium verification and search will be available when the API is connected."),
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(10f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };
            resultsPanel.Controls.Add(_emptyState);

            var footer = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Form.ApiPendingShort", "Waiting for the Premium API endpoints."),
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(8.5f, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleLeft
            };

            layout.Controls.Add(heading, 0, 0);
            layout.Controls.Add(description, 0, 1);
            layout.Controls.Add(searchLayout, 0, 2);
            layout.Controls.Add(resultsPanel, 0, 3);
            layout.Controls.Add(footer, 0, 4);

            content.Controls.Add(layout);
            content.Controls.Add(_premiumNotice);
            Controls.Add(content);
            Controls.Add(titleBar);

            SetPremiumAccess(false);
            ResumeLayout(true);
        }

        private Panel BuildTitleBar()
        {
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
                Text = Text,
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
            return titleBar;
        }

        private Panel BuildResultsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ElevatedColor,
                Padding = new Padding(1)
            };
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = ElevatedColor,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(51, 70, 85),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                Enabled = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeight = 38,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(28, 48, 63),
                    ForeColor = MutedTextColor,
                    Font = CreateUiFont(8.5f, FontStyle.Bold)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = ElevatedColor,
                    ForeColor = TextColor,
                    SelectionBackColor = Color.FromArgb(34, 79, 99),
                    SelectionForeColor = TextColor,
                    Font = CreateUiFont(9f, FontStyle.Regular)
                }
            };
            _resultsGrid = grid;
            grid.RowTemplate.Height = 42;
            grid.Columns.Add(CreateColumn(
                _kind == PremiumSearchKind.Playlists
                    ? GetLocalizedText("Premium.Grid.Playlist", "PLAYLIST")
                    : GetLocalizedText("Premium.Grid.Artist", "ARTIST"), 60));
            grid.Columns.Add(CreateColumn(
                _kind == PremiumSearchKind.Playlists
                    ? GetLocalizedText("Premium.Grid.Owner", "OWNER")
                    : GetLocalizedText("Premium.Grid.Matches", "MATCHES"), 28));
            grid.Columns.Add(CreateColumn(GetLocalizedText("Premium.Grid.Songs", "SONGS"), 18));
            panel.Controls.Add(grid);
            return panel;
        }

        private static DataGridViewTextBoxColumn CreateColumn(string header, float fillWeight)
        {
            return new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = fillWeight,
                ReadOnly = true
            };
        }

        private Panel BuildPremiumNotice()
        {
            var notice = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 92,
                BackColor = Color.FromArgb(91, 37, 50),
                Padding = new Padding(0)
            };
            var accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 6,
                BackColor = Color.FromArgb(235, 87, 103)
            };
            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(18, 10, 18, 10)
            };
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var title = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Form.LockedTitle", "PREMIUM ACCESS REQUIRED"),
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            var label = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Form.Locked", "This feature is reserved for Premium members. Sign in with a Premium account to use it."),
                ForeColor = Color.FromArgb(255, 225, 229),
                BackColor = Color.Transparent,
                Font = CreateUiFont(9f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            content.Controls.Add(title, 0, 0);
            content.Controls.Add(label, 0, 1);
            notice.Controls.Add(content);
            notice.Controls.Add(accent);
            return notice;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!_hasPremiumAccess || string.IsNullOrWhiteSpace(_searchTextBox.Text)) return;
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            _searchButton.PerformClick();
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
    }
}
