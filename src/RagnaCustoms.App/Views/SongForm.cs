using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using RagnaCustoms.App;
using RagnaCustoms.App.Extensions;
using RagnaCustoms.App.Properties;
using RagnaCustoms.App.Views;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using TwitchLib.PubSub.Models.Responses.Messages.AutomodCaughtMessage;
using Configuration = RagnaCustoms.Services.Configuration;

namespace RagnaCustoms.Views
{
    public partial class SongForm : Form, ISongView
    {
        private static readonly Color BackgroundColor = Color.FromArgb(8, 19, 29);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 31, 43);
        private static readonly Color SurfaceElevatedColor = Color.FromArgb(28, 42, 55);
        private static readonly Color BorderColor = Color.FromArgb(51, 70, 85);
        private static readonly Color TextColor = Color.FromArgb(239, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(137, 158, 174);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 216);
        private static readonly Color AccentDarkColor = Color.FromArgb(14, 105, 139);
        private static readonly Color InstalledRowColor = Color.FromArgb(22, 54, 57);

        private Configuration _configuration;
        private Panel _mainBody;
        private BorderPanel _heroPanel;
        private BorderPanel _resultsPanel;
        private Label _searchPlaceholder;

        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref TextRect lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct TextRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public SongForm()
        {
            InitializeComponent();
            SearchResultGridView.AutoGenerateColumns = false;
            _configuration= new Configuration();
            englishToolStripMenuItem.Checked = _configuration.Lang == "en";
            frenchToolStripMenuItem.Checked = _configuration.Lang == "fr";
            SetLoginMenuState(!String.IsNullOrEmpty(_configuration.ApiKey));

            Text += $" {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            ApplyVisualTheme();
        }

        public SongPresenter Presenter { private get; set; }

        public IEnumerable<SongSearchModel> Songs
        {
            get => (IEnumerable<SongSearchModel>)SearchResultGridView.DataSource;
            set => SearchResultGridView.DataSource = value;
        } 

   

        public virtual void ShowAsPopup()
        {
            ShowDialog();
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await Presenter.SearchOnlineAsync(SearchTextBox.Text);
        }

        private void SearchResultGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;         
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var song = (SongSearchModel)senderGrid.Rows[e.RowIndex].DataBoundItem;

                if (senderGrid.Columns[e.ColumnIndex].DataPropertyName == "Download")
                {
                    song.IsInstalled = true;
                    senderGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = InstalledRowColor;
                    Presenter.DownloadAsync(song.Id,song.CurrentFolder);
                }
                else if (senderGrid.Columns[e.ColumnIndex].DataPropertyName == "Delete")
                {
                    if (!song.IsInstalled)
                    {
                        return;
                    }

                    try
                    {
                        Directory.Delete(song.CurrentFolder, true);                            
                        senderGrid.Rows.Remove(senderGrid.Rows[e.RowIndex]);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(GetResourceText("Song.Form.DeleteError", "We couldn't delete this song. Please delete it manually from the custom songs directory."));
                    }
                }

            }

        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApiKeyMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ApiKey = Prompt.ShowDialog(Resources.Song_Form_EnterYourApiKey, "RagnaCustoms", Presenter.ApiKey);
        }


        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            Process.Start("explorer.exe", dir);
        }

        private void logScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var logsForm = new LogsForm();
            logsForm.Show();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void SendScoreAutomaticallyMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void checkAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var device = AndroidDevice.GetFirstFoundDevice();
            if (device != null)
                MessageBox.Show(
                    string.Format(Resources.Song_Form_CompatibleDeviceFound, device.Manufacturer, device.Description),
                    "RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(Resources.Song_Form_NoCompatibleDeviceFound, "RagnaCustoms", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void syncSongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = AndroidDevice.SyncSongs();
            if (result == 0)
                MessageBox.Show(Resources.Song_Form_SyncComplete, "RagnaCustoms", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else if (result == 1)
                MessageBox.Show(Resources.Song_Form_NoCompatibleDeviceFound, "RagnaCustoms", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void compareSongsVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CompareSongsAsync();
        }

        private void twitchBotToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TwitchBotForm.ShowInstance();
        }

        private void gotoOverlayUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Presenter.ApiKey))
            {
                MessageBox.Show(Resources.Song_Form_NeedToSetYourApiKeyFirst, "RagnaCustoms", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var sInfo = new ProcessStartInfo($"https://api.ragnacustoms.com/overlay/display/{Presenter.ApiKey}");
                Process.Start(sInfo);
            }
        }


        private void configureApiKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ApiKey = Prompt.ShowDialog(Resources.Song_Form_EnterYourApiKey, "RagnaCustoms", Presenter.ApiKey);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.Lang = "en";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Presenter.Lang, true);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            MessageBox.Show(this, GetResourceText("Song.Form.RestartMessage", "Please restart the application to apply the language change."), GetResourceText("Song.Form.RestartNeeded", "Restart needed"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void frenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.Lang = "fr";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Presenter.Lang, true);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            MessageBox.Show(this, GetResourceText("Song.Form.RestartMessage", "Please restart the application to apply the language change."),
                GetResourceText("Song.Form.RestartNeeded", "Restart needed"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SearchResultGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            foreach (DataGridViewRow row in senderGrid.Rows)
            {
                var song = row.DataBoundItem as SongSearchModel;
                if (song == null) continue;

                row.DefaultCellStyle.BackColor = song.IsInstalled
                    ? InstalledRowColor
                    : SurfaceColor;
                row.DefaultCellStyle.ForeColor = TextColor;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var dir = _configuration.BaseFolder ?? DirProvider.getCustomDirectory().FullName;
            Process.Start("explorer.exe", dir);
        }

        private void SongForm_Load(object sender, EventArgs e)
        {

        }

        private void downloadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {        
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Presenter.ApiKey = Prompt.ShowDialog(Resources.Song_Form_EnterYourApiKey, "RagnaCustoms", Presenter.ApiKey);

        }

        private void preferencesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var pref = new Preferences();
            pref.ShowDialog();
        }

        private void twitchBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwitchBotForm.ShowInstance();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_configuration.ApiKey))
            {
                new LoginForm(this).Show();
            }
            else
            {
                _configuration.ApiKey = "";
                SetLoginMenuState(false);
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSearchPlaceholder();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            SearchButton.PerformClick();
        }

        private void UpdateSearchPlaceholder()
        {
            if (_searchPlaceholder == null || SearchTextBox == null) return;
            _searchPlaceholder.Visible = string.IsNullOrEmpty(SearchTextBox.Text) && !SearchTextBox.Focused;
        }

        public void changeLoginMenu()
        {
            SetLoginMenuState(!string.IsNullOrEmpty(_configuration.ApiKey));
        }

        private void SetLoginMenuState(bool loggedIn)
        {
            loginToolStripMenuItem.Text = GetResourceText(loggedIn ? "Song.Form.Logout" : "Song.Form.Login", loggedIn ? "Logout" : "Login");

            var oldImage = loginToolStripMenuItem.Image;
            loginToolStripMenuItem.Image = CreateStatusDot(loggedIn
                ? Color.FromArgb(74, 207, 137)
                : Color.FromArgb(219, 76, 91));
            oldImage?.Dispose();
        }

        private static Bitmap CreateStatusDot(Color color)
        {
            var bitmap = new Bitmap(12, 12);
            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(color))
            using (var outline = new Pen(Color.FromArgb(8, 19, 29), 1))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);
                graphics.FillEllipse(brush, 2, 2, 8, 8);
                graphics.DrawEllipse(outline, 2, 2, 8, 8);
            }
            return bitmap;
        }

        private void helpPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("https://api.ragnacustoms.com/getting-started?from=app");
            Process.Start(sInfo);
        }

        private void imSureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryInfo customDirectory = DirProvider.getCustomDirectory();

            try
            {
                DeleteDirectoryContents(customDirectory.FullName);

                MessageBox.Show(GetResourceText("Song.Form.FolderCleared", "Folder cleared!"), "RagnaCustoms.com", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
            }catch(Exception o_O)
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

        private void ApplyVisualTheme()
        {
            SuspendLayout();

            BackColor = BackgroundColor;
            ForeColor = TextColor;
            Font = CreateUiFont(9.5f, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimumSize = new Size(920, 620);
            ClientSize = new Size(1180, 760);
            StartPosition = FormStartPosition.CenterScreen;

            ConfigureMenu();

            Controls.Remove(SearchTextBox);
            Controls.Remove(SearchButton);
            Controls.Remove(SearchResultGridView);
            Controls.Remove(Menu);

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = Color.FromArgb(13, 27, 39),
                Padding = new Padding(22, 0, 22, 0)
            };

            var logo = new PictureBox
            {
                Image = Resources.logocompressed,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(16, 6),
                Size = new Size(102, 30),
                BackColor = Color.Transparent
            };
            header.Controls.Add(logo);

            var headerTitle = new Label
            {
                AutoSize = true,
                Text = $"RagnaCustoms {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}",
                ForeColor = TextColor,
                Font = CreateUiFont(9.5f, FontStyle.Bold),
                Location = new Point(132, 8)
            };
            header.Controls.Add(headerTitle);

            var headerSubtitle = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.Manager", "CUSTOM SONG MANAGER"),
                ForeColor = AccentColor,
                Font = CreateUiFont(7f, FontStyle.Bold),
                Location = new Point(133, 24)
            };
            header.Controls.Add(headerSubtitle);

            var minimizeButton = CreateWindowButton("—", (sender, args) => WindowState = FormWindowState.Minimized);
            Button maximizeButton = null;
            maximizeButton = CreateWindowButton("□", (sender, args) =>
            {
                WindowState = WindowState == FormWindowState.Maximized
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;
                maximizeButton.Text = WindowState == FormWindowState.Maximized ? "❐" : "□";
            });
            var closeButton = CreateWindowButton("×", (sender, args) => Close());
            closeButton.BackColor = Color.FromArgb(13, 27, 39);
            closeButton.MouseEnter += (sender, args) => closeButton.BackColor = Color.FromArgb(150, 52, 68);
            closeButton.MouseLeave += (sender, args) => closeButton.BackColor = Color.FromArgb(13, 27, 39);

            header.Controls.Add(minimizeButton);
            header.Controls.Add(maximizeButton);
            header.Controls.Add(closeButton);
            Action positionWindowButtons = () =>
            {
                closeButton.Left = header.ClientSize.Width - closeButton.Width;
                maximizeButton.Left = closeButton.Left - maximizeButton.Width;
                minimizeButton.Left = maximizeButton.Left - minimizeButton.Width;
            };
            header.Resize += (sender, args) => positionWindowButtons();
            positionWindowButtons();
            AttachWindowDrag(header);

            _mainBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(22, 20, 22, 20)
            };

            BuildHeroPanel();
            BuildResultsPanel();

            _mainBody.Controls.Add(_resultsPanel);
            _mainBody.Controls.Add(_heroPanel);

            Controls.Add(_mainBody);
            Controls.Add(Menu);
            Controls.Add(header);
            Controls.SetChildIndex(_mainBody, 0);
            Controls.SetChildIndex(Menu, 1);
            Controls.SetChildIndex(header, 2);

            ResumeLayout(true);
        }

        private void BuildHeroPanel()
        {
            _heroPanel = new BorderPanel
            {
                Dock = DockStyle.Top,
                Height = 148,
                BackColor = SurfaceColor,
                BorderColor = BorderColor,
                Padding = new Padding(22, 18, 22, 18)
            };

            var eyebrow = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.Library", "CUSTOM SONG LIBRARY"),
                ForeColor = AccentColor,
                Font = CreateUiFont(8.5f, FontStyle.Bold),
                Location = new Point(22, 17)
            };
            _heroPanel.Controls.Add(eyebrow);

            var title = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.FindSong", "Find a song"),
                ForeColor = TextColor,
                Font = CreateUiFont(18f, FontStyle.Bold),
                Location = new Point(20, 38)
            };
            _heroPanel.Controls.Add(title);

            var description = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.Description", "Search, download and manage your custom songs for Ragnarock."),
                ForeColor = MutedTextColor,
                Font = CreateUiFont(9.5f, FontStyle.Regular),
                Location = new Point(22, 73)
            };
            _heroPanel.Controls.Add(description);

            var searchSurface = new BorderPanel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Height = 42,
                Location = new Point(22, 94),
                BackColor = Color.FromArgb(22, 37, 49),
                BorderColor = Color.FromArgb(49, 68, 82),
                Padding = new Padding(0)
            };

            var inputFrame = new BorderPanel
            {
                Height = 40,
                Location = new Point(0, 1),
                BackColor = Color.FromArgb(22, 37, 49),
                BorderColor = Color.FromArgb(22, 37, 49),
                Padding = new Padding(0)
            };

            SearchTextBox.BorderStyle = BorderStyle.None;
            SearchTextBox.BackColor = Color.FromArgb(22, 37, 49);
            SearchTextBox.ForeColor = TextColor;
            SearchTextBox.Font = CreateUiFont(11f, FontStyle.Regular);
            SearchTextBox.Multiline = false;
            SearchTextBox.AutoSize = false;
            SearchTextBox.Height = 26;
            SearchTextBox.Location = new Point(14, 7);
            SearchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SearchTextBox.Margin = new Padding(0);
            SearchTextBox.KeyDown -= SearchTextBox_KeyDown;
            SearchTextBox.KeyDown += SearchTextBox_KeyDown;
            inputFrame.Controls.Add(SearchTextBox);

            _searchPlaceholder = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Text = GetResourceText("Song.Form.SearchPlaceholder", "Search a song by name, author or mapper…"),
                ForeColor = MutedTextColor,
                BackColor = Color.FromArgb(22, 37, 49),
                Font = CreateUiFont(10.5f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                Cursor = Cursors.IBeam
            };
            _searchPlaceholder.Click += (sender, args) => SearchTextBox.Focus();
            inputFrame.Controls.Add(_searchPlaceholder);
            SearchTextBox.Enter += (sender, args) => UpdateSearchPlaceholder();
            SearchTextBox.Leave += (sender, args) => UpdateSearchPlaceholder();

            SearchButton.Text = GetResourceText("Song.Form.Search", "SEARCH").ToUpperInvariant();
            SearchButton.Height = 40;
            SearchButton.Width = 124;
            SearchButton.Location = new Point(0, 1);
            SearchButton.FlatStyle = FlatStyle.Flat;
            SearchButton.FlatAppearance.BorderSize = 0;
            SearchButton.BackColor = AccentColor;
            SearchButton.ForeColor = Color.FromArgb(6, 24, 34);
            SearchButton.Font = CreateUiFont(8.5f, FontStyle.Bold);
            SearchButton.UseVisualStyleBackColor = false;
            searchSurface.Controls.Add(inputFrame);
            searchSurface.Controls.Add(SearchButton);

            _heroPanel.Controls.Add(searchSurface);
            Action layoutSearchSurface = () =>
            {
                searchSurface.Width = _heroPanel.ClientSize.Width - 44;
                inputFrame.Width = searchSurface.ClientSize.Width - SearchButton.Width;
                SearchTextBox.Width = inputFrame.ClientSize.Width - SearchTextBox.Left - 8;
                SearchButton.Left = searchSurface.ClientSize.Width - SearchButton.Width;
            };
            _heroPanel.Resize += (sender, args) => layoutSearchSurface();
            searchSurface.Resize += (sender, args) => layoutSearchSurface();
            layoutSearchSurface();
            UpdateSearchPlaceholder();
        }

        private void BuildResultsPanel()
        {
            _resultsPanel = new BorderPanel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                BorderColor = BorderColor,
                Padding = new Padding(14, 0, 14, 14)
            };

            var resultsHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = SurfaceColor
            };

            var resultsTitle = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.Results", "RESULTS"),
                ForeColor = TextColor,
                Font = CreateUiFont(10f, FontStyle.Bold),
                Location = new Point(4, 18)
            };
            resultsHeader.Controls.Add(resultsTitle);

            var resultsHint = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.ResultsHint", "Available songs will appear here"),
                ForeColor = MutedTextColor,
                Font = CreateUiFont(8.5f, FontStyle.Regular),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(330, 20)
            };
            resultsHeader.Controls.Add(resultsHint);
            resultsHeader.Resize += (sender, args) => resultsHint.Left = resultsHeader.ClientSize.Width - resultsHint.Width - 4;

            var legendPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 34,
                BackColor = SurfaceColor,
                Padding = new Padding(4, 0, 4, 0)
            };

            var legendTitle = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.Legend", "Legend"),
                ForeColor = MutedTextColor,
                Font = CreateUiFont(8f, FontStyle.Bold),
                Location = new Point(4, 10)
            };
            legendPanel.Controls.Add(legendTitle);

            var legendSwatch = new Panel
            {
                Size = new Size(16, 16),
                Location = new Point(legendTitle.Right + 12, 8),
                BackColor = InstalledRowColor
            };
            legendPanel.Controls.Add(legendSwatch);

            var legendText = new Label
            {
                AutoSize = true,
                Text = GetResourceText("Song.Form.AlreadyPresent", "Already present locally"),
                ForeColor = TextColor,
                Font = CreateUiFont(8.5f, FontStyle.Regular),
                Location = new Point(legendSwatch.Right + 7, 9)
            };
            legendPanel.Controls.Add(legendText);

            ConfigureResultsGrid();
            _resultsPanel.Controls.Add(SearchResultGridView);
            _resultsPanel.Controls.Add(legendPanel);
            _resultsPanel.Controls.Add(resultsHeader);
        }

        private void ConfigureResultsGrid()
        {
            SearchResultGridView.Dock = DockStyle.Fill;
            SearchResultGridView.BackgroundColor = SurfaceColor;
            SearchResultGridView.BorderStyle = BorderStyle.None;
            SearchResultGridView.GridColor = BorderColor;
            SearchResultGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            SearchResultGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            SearchResultGridView.EnableHeadersVisualStyles = false;
            SearchResultGridView.ColumnHeadersHeight = 38;
            SearchResultGridView.RowTemplate.Height = 46;
            SearchResultGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SearchResultGridView.MultiSelect = false;
            SearchResultGridView.AllowUserToResizeRows = false;
            SearchResultGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            SearchResultGridView.CellPainting -= SearchResultGridView_CellPainting;
            SearchResultGridView.CellPainting += SearchResultGridView_CellPainting;
            SearchResultGridView.CellFormatting -= SearchResultGridView_CellFormatting;
            SearchResultGridView.CellFormatting += SearchResultGridView_CellFormatting;

            SearchResultGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SurfaceElevatedColor,
                ForeColor = TextColor,
                SelectionBackColor = SurfaceElevatedColor,
                SelectionForeColor = TextColor,
                Font = CreateUiFont(8.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            SearchResultGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SurfaceColor,
                ForeColor = TextColor,
                SelectionBackColor = AccentDarkColor,
                SelectionForeColor = TextColor,
                Font = CreateUiFont(9f, FontStyle.Regular),
                Padding = new Padding(8, 0, 4, 0)
            };
            SearchResultGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SurfaceColor,
                ForeColor = TextColor,
                SelectionBackColor = AccentDarkColor,
                SelectionForeColor = TextColor
            };

            Id.Visible = false;
            SongName.FillWeight = 150;
            SongDifficulties.FillWeight = 80;
            SongAuthor.FillWeight = 90;
            SongMapper.FillWeight = 90;
            SongDownload.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SongDownload.FillWeight = 105;
            SongDownload.MinimumWidth = 120;
            SongDownload.Text = GetResourceText("Song.Form.Download", "DOWNLOAD").ToUpperInvariant();
            Delete.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Delete.FillWeight = 90;
            Delete.MinimumWidth = 105;
            Delete.Text = GetResourceText("Song.Form.Delete", "DELETE").ToUpperInvariant();

            SongDownload.DefaultCellStyle = CreateGridButtonStyle(AccentColor, Color.FromArgb(6, 24, 34));
            Delete.DefaultCellStyle = CreateGridButtonStyle(Color.FromArgb(136, 54, 69), TextColor);
        }

        private void SearchResultGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= SearchResultGridView.Rows.Count) return;

            var song = SearchResultGridView.Rows[e.RowIndex].DataBoundItem as SongSearchModel;
            if (song == null) return;

            if (e.ColumnIndex == Delete.Index && !song.IsInstalled)
            {
                e.Value = string.Empty;
            }

            var rowColor = song.IsInstalled ? InstalledRowColor : SurfaceColor;
            e.CellStyle.BackColor = rowColor;
            e.CellStyle.ForeColor = TextColor;
            e.CellStyle.SelectionBackColor = rowColor;
            e.CellStyle.SelectionForeColor = TextColor;
        }

        private static DataGridViewCellStyle CreateGridButtonStyle(Color backColor, Color foreColor)
        {
            return new DataGridViewCellStyle
            {
                BackColor = backColor,
                ForeColor = foreColor,
                SelectionBackColor = backColor,
                SelectionForeColor = foreColor,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = CreateUiFont(8f, FontStyle.Bold)
            };
        }

        private void ConfigureMenu()
        {
            Menu.Dock = DockStyle.Top;
            Menu.Height = 34;
            Menu.BackColor = Color.FromArgb(13, 27, 39);
            Menu.ForeColor = TextColor;
            Menu.RenderMode = ToolStripRenderMode.Professional;
            Menu.Renderer = new RagnaMenuRenderer();
            Menu.Padding = new Padding(18, 0, 0, 0);

            foreach (ToolStripItem item in Menu.Items)
            {
                item.Font = CreateUiFont(9f, FontStyle.Regular);
                item.ForeColor = TextColor;
                item.Padding = new Padding(10, 0, 10, 0);
            }

            loginToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
        }

        private void SearchResultGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (!(SearchResultGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) return;

            e.PaintBackground(e.CellBounds, true);

            var song = SearchResultGridView.Rows[e.RowIndex].DataBoundItem as SongSearchModel;
            if (e.ColumnIndex == Delete.Index && (song == null || !song.IsInstalled))
            {
                e.Handled = true;
                return;
            }

            var buttonBounds = Rectangle.Inflate(e.CellBounds, -1, -2);
            var buttonColor = e.ColumnIndex == SongDownload.Index
                ? AccentColor
                : Color.FromArgb(136, 54, 69);
            var textColor = e.ColumnIndex == SongDownload.Index
                ? Color.FromArgb(6, 24, 34)
                : TextColor;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = CreateRoundedRectangle(buttonBounds, 4))
            using (var brush = new SolidBrush(buttonColor))
            using (var textBrush = new SolidBrush(textColor))
            using (var font = CreateUiFont(8f, FontStyle.Bold))
            using (var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawString(
                    e.ColumnIndex == SongDownload.Index
                        ? GetResourceText("Song.Form.Download", "DOWNLOAD").ToUpperInvariant()
                        : GetResourceText("Song.Form.Delete", "DELETE").ToUpperInvariant(),
                    font,
                    textBrush,
                    buttonBounds,
                    format);
            }

            e.Handled = true;
        }

        private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            var diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Button CreateWindowButton(string text, EventHandler click)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(44, 42),
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(13, 27, 39),
                ForeColor = TextColor,
                Font = CreateUiFont(text == "×" ? 16f : 11f, FontStyle.Regular),
                TabStop = false,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += click;
            button.MouseEnter += (sender, args) =>
            {
                if (text != "×") button.BackColor = SurfaceElevatedColor;
            };
            button.MouseLeave += (sender, args) =>
            {
                if (text != "×") button.BackColor = Color.FromArgb(13, 27, 39);
            };
            return button;
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

        private static Font CreateUiFont(float size, FontStyle style)
        {
            return new Font("Bahnschrift", size, style, GraphicsUnit.Point);
        }

        private static string GetResourceText(string key, string fallback)
        {
            return Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }

        private sealed class BorderPanel : Panel
        {
            public Color BorderColor { get; set; }

            public BorderPanel()
            {
                DoubleBuffered = true;
                SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                using (var pen = new Pen(BorderColor, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                }
            }
        }

        private sealed class CenteredTextBox : TextBox
        {
            private const int EmSetRect = 0xB3;

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                CenterText();
            }

            protected override void OnFontChanged(EventArgs e)
            {
                base.OnFontChanged(e);
                CenterText();
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                CenterText();
            }

            private void CenterText()
            {
                if (!IsHandleCreated || Multiline) return;

                var textHeight = TextRenderer.MeasureText("Ag", Font).Height;
                var verticalPadding = Math.Max(0, (ClientSize.Height - textHeight) / 2);
                var rect = new TextRect
                {
                    Left = 0,
                    Top = verticalPadding,
                    Right = ClientSize.Width,
                    Bottom = ClientSize.Height - verticalPadding
                };
                SendMessage(Handle, EmSetRect, IntPtr.Zero, ref rect);
            }
        }

        private sealed class RagnaMenuRenderer : ToolStripProfessionalRenderer
        {
            public RagnaMenuRenderer() : base(new RagnaColorTable())
            {
                RoundedEdges = false;
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = TextColor;
                base.OnRenderItemText(e);
            }
        }

        private sealed class RagnaColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => Color.FromArgb(13, 27, 39);
            public override Color MenuStripGradientEnd => Color.FromArgb(13, 27, 39);
            public override Color ToolStripDropDownBackground => SurfaceColor;
            public override Color ImageMarginGradientBegin => SurfaceColor;
            public override Color ImageMarginGradientMiddle => SurfaceColor;
            public override Color ImageMarginGradientEnd => SurfaceColor;
            public override Color MenuItemSelected => AccentDarkColor;
            public override Color MenuItemSelectedGradientBegin => AccentDarkColor;
            public override Color MenuItemSelectedGradientEnd => AccentDarkColor;
            public override Color MenuItemPressedGradientBegin => AccentDarkColor;
            public override Color MenuItemPressedGradientMiddle => AccentDarkColor;
            public override Color MenuItemPressedGradientEnd => AccentDarkColor;
            public override Color SeparatorDark => BorderColor;
            public override Color SeparatorLight => BorderColor;
        }
    }
}
