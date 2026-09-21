using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.Services;

namespace RagnaCustoms.App.Views
{
    public sealed class PremiumSongsPreviewForm : Form
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        private readonly PremiumSearchKind _kind;
        private readonly PremiumSearchResult _parentResult;
        private readonly PremiumSearchService _searchService;
        private readonly Action<IEnumerable<string>> _downloadSongs;
        private readonly List<SongCard> _songCards = new List<SongCard>();
        private FlowLayoutPanel _songsGrid;
        private Label _statusLabel;
        private Button _gridViewButton;
        private Button _listViewButton;
        private Button _selectAllButton;
        private Button _clearButton;
        private Button _downloadButton;
        private bool _isGridMode = true;

        private sealed class SongCard
        {
            public PremiumSongResult Song { get; set; }
            public Panel Panel { get; set; }
            public PictureBox Cover { get; set; }
            public CheckBox Selection { get; set; }
            public Label Title { get; set; }
            public Label Artist { get; set; }
            public Label Mapper { get; set; }
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public PremiumSongsPreviewForm(
            PremiumSearchKind kind,
            PremiumSearchResult parentResult,
            PremiumSearchService searchService,
            Action<IEnumerable<string>> downloadSongs)
        {
            _kind = kind;
            _parentResult = parentResult;
            _searchService = searchService;
            _downloadSongs = downloadSongs;
            BuildForm();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadSongsAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var card in _songCards)
                {
                    card.Cover?.Image?.Dispose();
                }
                _songCards.Clear();
            }

            base.Dispose(disposing);
        }

        private void BuildForm()
        {
            SuspendLayout();
            Text = string.Format(
                CultureInfo.CurrentCulture,
                GetLocalizedText("Premium.Preview.Title", "Song preview - {0}"),
                _parentResult.Name);
            BackColor = WindowColor;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ClientSize = new Size(760, 560);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;

            var titleBar = BuildTitleBar();
            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(28, 24, 28, 24)
            };
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 4
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

            var headingBar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            var heading = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Preview.Heading", "SELECT SONGS TO DOWNLOAD"),
                ForeColor = AccentColor,
                Font = CreateUiFont(10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            _listViewButton = CreateViewButton("☰", "LIST");
            _listViewButton.Click += (sender, args) => SetViewMode(false);
            _gridViewButton = CreateViewButton("▦", "GRID");
            _gridViewButton.Click += (sender, args) => SetViewMode(true);
            headingBar.Controls.Add(heading);
            headingBar.Controls.Add(_listViewButton);
            headingBar.Controls.Add(_gridViewButton);
            _statusLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("Premium.Preview.Loading", "Loading songs..."),
                ForeColor = MutedTextColor,
                Font = CreateUiFont(9f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            _songsGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = InputColor,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(8)
            };
            _songsGrid.Resize += (sender, args) => LayoutSongCards();

            var actions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 4,
                RowCount = 1
            };
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));

            _selectAllButton = CreateButton(GetLocalizedText("Premium.Preview.SelectAll", "SELECT ALL"));
            _selectAllButton.Click += (sender, args) => SetAllChecked(true);
            _clearButton = CreateButton(GetLocalizedText("Premium.Preview.Clear", "REMOVE ALL"));
            _clearButton.Click += (sender, args) => SetAllChecked(false);
            _downloadButton = CreateButton(GetLocalizedText("Premium.Preview.Download", "DOWNLOAD SELECTED"));
            _downloadButton.Click += DownloadButton_Click;
            actions.Controls.Add(_selectAllButton, 1, 0);
            actions.Controls.Add(_clearButton, 2, 0);
            actions.Controls.Add(_downloadButton, 3, 0);

            layout.Controls.Add(headingBar, 0, 0);
            layout.Controls.Add(_statusLabel, 0, 1);
            layout.Controls.Add(_songsGrid, 0, 2);
            layout.Controls.Add(actions, 0, 3);
            content.Controls.Add(layout);
            Controls.Add(content);
            Controls.Add(titleBar);
            SetViewMode(true);
            SetActionState(false);
            ResumeLayout(true);
        }

        private async void LoadSongsAsync()
        {
            try
            {
                var songs = _kind == PremiumSearchKind.Playlists
                    ? await _searchService.GetPlaylistSongsAsync(_parentResult.Id)
                    : await _searchService.GetArtistSongsAsync(_parentResult.Id);
                if (IsDisposed) return;

                if (_kind == PremiumSearchKind.Artists)
                {
                    foreach (var song in songs)
                    {
                        if (string.IsNullOrWhiteSpace(song.Artist)) song.Artist = _parentResult.Name;
                    }
                }

                await EnrichSongDetailsAsync(songs);
                if (IsDisposed) return;

                _songCards.Clear();
                _songsGrid.Controls.Clear();
                foreach (var song in songs)
                {
                    var card = CreateSongCard(song);
                    _songCards.Add(card);
                    _songsGrid.Controls.Add(card.Panel);
                }
                LayoutSongCards();

                _statusLabel.ForeColor = MutedTextColor;
                _statusLabel.Text = string.Format(
                    CultureInfo.CurrentCulture,
                    GetLocalizedText("Premium.Preview.Count", "{0} song(s) loaded. Select what you want to download."),
                    songs.Count);
                SetActionState(songs.Count > 0);

                _ = LoadCoversAsync(songs);
            }
            catch (PremiumSearchException exception)
            {
                TwitchBotLogger.Error(
                    "Unable to load Premium song preview. Detail: " + exception.Detail,
                    exception);
                ShowError(exception.StatusCode);
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Unable to load Premium song preview.", exception);
                ShowError(HttpStatusCode.ServiceUnavailable);
            }
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            var selectedIds = new List<string>();
            foreach (var card in _songCards)
            {
                if (card.Selection.Checked && !string.IsNullOrWhiteSpace(card.Song.Id))
                {
                    selectedIds.Add(card.Song.Id);
                }
            }

            if (selectedIds.Count == 0)
            {
                _statusLabel.ForeColor = Color.FromArgb(255, 166, 79);
                _statusLabel.Text = GetLocalizedText("Premium.Preview.NoneSelected", "Select at least one song.");
                return;
            }

            _downloadSongs(selectedIds);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SetAllChecked(bool isChecked)
        {
            foreach (var card in _songCards)
            {
                card.Selection.Checked = isChecked;
            }
        }

        private async Task EnrichSongDetailsAsync(List<PremiumSongResult> songs)
        {
            var tasks = new List<Task>();
            foreach (var song in songs)
            {
                tasks.Add(EnrichSongDetailsAsync(song));
            }

            await Task.WhenAll(tasks);
        }

        private async Task EnrichSongDetailsAsync(PremiumSongResult song)
        {
            try
            {
                var details = await _searchService.GetSongDetailsAsync(song.Id);
                if (!string.IsNullOrWhiteSpace(details.Artist)) song.Artist = details.Artist;
                if (!string.IsNullOrWhiteSpace(details.Mapper)) song.Mapper = details.Mapper;
                song.CoverUrl = details.CoverUrl;
                if (string.IsNullOrWhiteSpace(song.CoverUrl))
                {
                    song.CoverUrl = "/covers/" + song.Id + ".webp";
                }
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Unable to load details for Premium song " + song.Id + ".", exception);
                song.CoverUrl = "/covers/" + song.Id + ".webp";
            }
        }

        private async Task LoadCoversAsync(List<PremiumSongResult> songs)
        {
            for (var index = 0; index < songs.Count; index++)
            {
                try
                {
                    var image = await _searchService.DownloadCoverAsync(songs[index].CoverUrl);
                    if (image == null) continue;
                    if (IsDisposed)
                    {
                        image.Dispose();
                        continue;
                    }

                    if (index < _songCards.Count)
                    {
                        var oldImage = _songCards[index].Cover.Image;
                        _songCards[index].Cover.Image = image;
                        oldImage?.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    TwitchBotLogger.Error("Unable to load cover for Premium song " + songs[index].Id + ".", exception);
                }
            }
        }

        private void SetActionState(bool enabled)
        {
            _gridViewButton.Enabled = enabled;
            _listViewButton.Enabled = enabled;
            _selectAllButton.Enabled = enabled;
            _clearButton.Enabled = enabled;
            _downloadButton.Enabled = enabled;
        }

        private void SetViewMode(bool isGridMode)
        {
            _isGridMode = isGridMode;
            _songsGrid.FlowDirection = isGridMode
                ? FlowDirection.LeftToRight
                : FlowDirection.TopDown;
            _songsGrid.WrapContents = isGridMode;
            _gridViewButton.BackColor = isGridMode ? AccentColor : InputColor;
            _gridViewButton.ForeColor = isGridMode ? Color.FromArgb(6, 24, 34) : TextColor;
            _listViewButton.BackColor = isGridMode ? InputColor : AccentColor;
            _listViewButton.ForeColor = isGridMode ? TextColor : Color.FromArgb(6, 24, 34);
            LayoutSongCards();
        }

        private void LayoutSongCards()
        {
            if (_songsGrid == null) return;

            var contentWidth = Math.Max(
                200,
                _songsGrid.ClientSize.Width - _songsGrid.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth - 4);
            foreach (var card in _songCards)
            {
                card.Panel.Margin = new Padding(_isGridMode ? 4 : 3);
                card.Panel.Padding = new Padding(_isGridMode ? 8 : 6);
                card.Panel.Width = _isGridMode ? 204 : contentWidth;
                card.Panel.Height = _isGridMode ? 246 : 78;
                LayoutSongCard(card);
            }
            _songsGrid.PerformLayout();
        }

        private void LayoutSongCard(SongCard card)
        {
            var content = card.Panel.DisplayRectangle;
            card.Cover.Dock = DockStyle.None;
            card.Selection.Dock = DockStyle.None;
            card.Title.Dock = DockStyle.None;
            card.Artist.Dock = DockStyle.None;
            card.Mapper.Dock = DockStyle.None;
            if (_isGridMode)
            {
                var coverHeight = 136;
                card.Cover.SetBounds(content.X, content.Y, content.Width, coverHeight);
                card.Selection.SetBounds(content.X, content.Y + coverHeight, content.Width, 25);
                card.Title.SetBounds(content.X, content.Y + coverHeight + 25, content.Width, 29);
                card.Artist.SetBounds(content.X, content.Y + coverHeight + 54, content.Width, 22);
                card.Mapper.SetBounds(content.X, content.Y + coverHeight + 76, content.Width, 20);
                return;
            }

            var coverWidth = 64;
            var selectionWidth = 84;
            var textX = content.X + coverWidth + 8;
            var textWidth = Math.Max(80, content.Width - coverWidth - selectionWidth - 14);
            card.Cover.SetBounds(content.X, content.Y, coverWidth, content.Height);
            card.Selection.SetBounds(content.Right - selectionWidth, content.Y, selectionWidth, 22);
            card.Title.SetBounds(textX, content.Y, textWidth, 22);
            card.Artist.SetBounds(textX, content.Y + 22, textWidth, 18);
            card.Mapper.SetBounds(textX, content.Y + 40, textWidth, 18);
        }

        private void ShowError(HttpStatusCode statusCode)
        {
            if (IsDisposed) return;
            SetActionState(false);
            _statusLabel.ForeColor = Color.FromArgb(255, 149, 163);
            if ((int)statusCode >= 500)
            {
                _statusLabel.Text = string.Format(
                    CultureInfo.CurrentCulture,
                    GetLocalizedText(
                        "Premium.Form.ServerError",
                        "The server returned an error (HTTP {0}). Please contact RagnaCustoms support."),
                    (int)statusCode);
            }
            else
            {
                _statusLabel.Text = GetLocalizedText(
                    "Premium.Form.ApiError",
                    "The search API could not be reached. Please contact RagnaCustoms support.");
            }
        }

        private SongCard CreateSongCard(PremiumSongResult song)
        {
            var card = new Panel
            {
                Width = 214,
                Height = 246,
                Margin = new Padding(6),
                Padding = new Padding(8),
                BackColor = SurfaceColor,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            var cover = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 138,
                BackColor = Color.FromArgb(28, 48, 63),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = CreateCoverPlaceholder(),
                Cursor = Cursors.Hand
            };
            var selection = new CheckBox
            {
                Dock = DockStyle.Top,
                Height = 25,
                AutoSize = false,
                Checked = true,
                Text = GetLocalizedText("Premium.Preview.Select", "DOWNLOAD"),
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            var title = CreateCardLabel(song.Name, TextColor, 9.5f, FontStyle.Bold, 30);
            var artist = CreateCardLabel(
                string.Format(
                    CultureInfo.CurrentCulture,
                    GetLocalizedText("Premium.Preview.ArtistValue", "Artist: {0}"),
                    string.IsNullOrWhiteSpace(song.Artist) ? "-" : song.Artist),
                MutedTextColor,
                8.5f,
                FontStyle.Regular,
                22);
            var mapper = CreateCardLabel(
                string.Format(
                    CultureInfo.CurrentCulture,
                    GetLocalizedText("Premium.Preview.MapperValue", "Mapper: {0}"),
                    string.IsNullOrWhiteSpace(song.Mapper) ? "-" : song.Mapper),
                MutedTextColor,
                8.5f,
                FontStyle.Italic,
                20);

            card.Controls.Add(mapper);
            card.Controls.Add(artist);
            card.Controls.Add(title);
            card.Controls.Add(selection);
            card.Controls.Add(cover);

            var songCard = new SongCard
            {
                Song = song,
                Panel = card,
                Cover = cover,
                Selection = selection,
                Title = title,
                Artist = artist,
                Mapper = mapper
            };
            AttachCardClick(card, songCard);
            AttachCardClick(cover, songCard);
            AttachCardClick(title, songCard);
            AttachCardClick(artist, songCard);
            AttachCardClick(mapper, songCard);
            return songCard;
        }

        private void AttachCardClick(Control control, SongCard card)
        {
            control.Click += (sender, args) => card.Selection.Checked = !card.Selection.Checked;
        }

        private static Label CreateCardLabel(string text, Color color, float size, FontStyle style, int height)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = height,
                Text = string.IsNullOrWhiteSpace(text) ? "-" : text,
                ForeColor = color,
                BackColor = Color.Transparent,
                Font = CreateUiFont(size, style),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(2, 0, 2, 0),
                Cursor = Cursors.Hand
            };
        }

        private static Bitmap CreateCoverPlaceholder()
        {
            var bitmap = new Bitmap(180, 138);
            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(Color.FromArgb(39, 64, 80)))
            using (var pen = new Pen(Color.FromArgb(76, 112, 131), 2f))
            {
                graphics.Clear(Color.FromArgb(28, 48, 63));
                graphics.FillRectangle(brush, 34, 25, 112, 88);
                graphics.DrawRectangle(pen, 34, 25, 112, 88);
                graphics.DrawLine(pen, 48, 98, 76, 70);
                graphics.DrawLine(pen, 76, 70, 98, 90);
                graphics.DrawLine(pen, 98, 90, 116, 72);
                graphics.FillEllipse(brush, 116, 38, 12, 12);
            }

            return bitmap;
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
            var title = new Label
            {
                Dock = DockStyle.Fill,
                Text = Text,
                ForeColor = TextColor,
                Font = CreateUiFont(10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0)
            };
            var closeButton = new Button
            {
                Dock = DockStyle.Right,
                Width = 42,
                Text = "X",
                FlatStyle = FlatStyle.Flat,
                BackColor = WindowColor,
                ForeColor = MutedTextColor,
                Font = CreateUiFont(12f, FontStyle.Bold),
                TabStop = false
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(151, 53, 64);
            closeButton.Click += (sender, args) => Close();
            titleBar.Controls.Add(title);
            titleBar.Controls.Add(closeButton);
            AttachWindowDrag(titleBar);
            AttachWindowDrag(title);
            return titleBar;
        }

        private static Button CreateButton(string text)
        {
            var button = new Button
            {
                Dock = DockStyle.Fill,
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentColor,
                ForeColor = Color.FromArgb(6, 24, 34),
                Font = CreateUiFont(8f, FontStyle.Bold),
                Margin = new Padding(6, 5, 0, 5)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private static Button CreateViewButton(string icon, string accessibleName)
        {
            var button = new Button
            {
                Dock = DockStyle.Right,
                Width = 34,
                Height = 26,
                Text = icon,
                FlatStyle = FlatStyle.Flat,
                BackColor = InputColor,
                ForeColor = TextColor,
                Font = new Font("Segoe UI Symbol", 13f, FontStyle.Regular, GraphicsUnit.Point),
                TabStop = false,
                AccessibleName = accessibleName,
                Cursor = Cursors.Hand,
                Margin = new Padding(3, 3, 0, 3)
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(64, 94, 112);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 92, 112);
            return button;
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
