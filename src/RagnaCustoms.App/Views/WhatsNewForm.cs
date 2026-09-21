using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;

namespace RagnaCustoms.App.Views
{
    public sealed class WhatsNewForm : Form
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color FeatureColor = Color.FromArgb(24, 43, 58);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        private sealed class ReleaseNotes
        {
            public ReleaseNotes(string version, string[] featureKeys, string[] featureFallbacks)
            {
                Version = version;
                FeatureKeys = featureKeys;
                FeatureFallbacks = featureFallbacks;
            }

            public string Version { get; private set; }
            public string[] FeatureKeys { get; private set; }
            public string[] FeatureFallbacks { get; private set; }
        }

        private static readonly ReleaseNotes[] KnownReleases =
        {
            new ReleaseNotes(
                "2.9.2",
                new[]
                {
                    "WhatsNew.Release.2.9.2.Item.Premium",
                    "WhatsNew.Release.2.9.2.Item.Playlists",
                    "WhatsNew.Release.2.9.2.Item.Downloads"
                },
                new[]
                {
                    "Your Premium status is now visible next to Login and Logout.",
                    "Premium playlist and artist search pages are now available.",
                    "Playlist downloads and download progress are easier to follow."
                }),
            new ReleaseNotes(
                "2.9.1",
                new[]
                {
                    "WhatsNew.Item.Stability",
                    "WhatsNew.Item.Queue",
                    "WhatsNew.Item.ClearQueue",
                    "WhatsNew.Item.Logs"
                },
                new[]
                {
                    "Chat messages are no longer sent twice after a restart.",
                    "Your Twitch request queue is now restored after a restart.",
                    "The queue can be cleared from the application or with the moderator command !clearqueue.",
                    "Diagnostic logs make connection and crash issues easier to investigate."
                })
        };

        private ComboBox _versionSelector;
        private TableLayoutPanel _featureList;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public WhatsNewForm(string version)
        {
            DoNotShowAgain = false;
            BuildForm(version);
        }

        public bool DoNotShowAgain { get; private set; }

        private void BuildForm(string version)
        {
            SuspendLayout();

            Text = GetLocalizedText("WhatsNew.Form.Title", "RagnaCustoms  ·  What's new");
            BackColor = WindowColor;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ClientSize = new Size(640, 430);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;

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
            closeButton.Click += (sender, args) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

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
                Padding = new Padding(28, 24, 28, 22)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 6
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));

            var heading = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("WhatsNew.Form.Heading", "WHAT'S NEW"),
                ForeColor = AccentColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            var intro = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("WhatsNew.Form.Intro", "Here’s what changed in this update."),
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(12f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            var versionSelectorPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };
            versionSelectorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            versionSelectorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));

            var versionLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("WhatsNew.Form.SelectVersion", "Version to view"),
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(9f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            _versionSelector = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = FeatureColor,
                ForeColor = TextColor,
                FlatStyle = FlatStyle.Flat,
                Font = CreateUiFont(9f, FontStyle.Regular),
                Margin = new Padding(0)
            };
            foreach (var release in GetReleaseHistory(version))
            {
                _versionSelector.Items.Add(release.Version);
            }
            _versionSelector.SelectedIndexChanged += (sender, args) =>
            {
                if (_versionSelector.SelectedItem != null)
                {
                    RefreshFeatureList((string)_versionSelector.SelectedItem);
                }
            };
            versionSelectorPanel.Controls.Add(versionLabel, 0, 0);
            versionSelectorPanel.Controls.Add(_versionSelector, 1, 0);

            _featureList = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 1,
                Padding = new Padding(0, 2, 0, 0)
            };
            var featureList = _featureList;

            var featureKeys = new[]
            {
                "WhatsNew.Item.Stability",
                "WhatsNew.Item.Queue",
                "WhatsNew.Item.ClearQueue",
                "WhatsNew.Item.Logs"
            };
            var featureFallbacks = new[]
            {
                "Chat messages are no longer sent twice after a restart.",
                "Your Twitch request queue is now restored after a restart.",
                "The queue can be cleared from the application or with the moderator command !clearqueue.",
                "Diagnostic logs make connection and crash issues easier to investigate."
            };
            for (var index = 0; index < featureKeys.Length; index++)
            {
                var feature = new Label
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 3, 0, 3),
                    Padding = new Padding(14, 0, 12, 0),
                    Text = "•  " + GetLocalizedText(featureKeys[index], featureFallbacks[index]),
                    ForeColor = TextColor,
                    BackColor = FeatureColor,
                    Font = CreateUiFont(10f, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                featureList.Controls.Add(feature, 0, index);
            }

            var thanks = new Label
            {
                Dock = DockStyle.Fill,
                Text = GetLocalizedText("WhatsNew.Form.Thanks", "Thanks for your feedback!"),
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = CreateUiFont(9f, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var actions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 3,
                RowCount = 1
            };
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 156));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92));

            var okButton = CreateButton(GetLocalizedText("WhatsNew.Form.Ok", "OK"));
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += (sender, args) => Close();

            var neverShowButton = CreateButton(
                GetLocalizedText("WhatsNew.Form.NeverShowAgain", "Don't show again"));
            neverShowButton.Click += (sender, args) =>
            {
                DoNotShowAgain = true;
                DialogResult = DialogResult.OK;
                Close();
            };

            actions.Controls.Add(neverShowButton, 1, 0);
            actions.Controls.Add(okButton, 2, 0);

            layout.Controls.Add(heading, 0, 0);
            layout.Controls.Add(intro, 0, 1);
            layout.Controls.Add(versionSelectorPanel, 0, 2);
            layout.Controls.Add(_featureList, 0, 3);
            layout.Controls.Add(thanks, 0, 4);
            layout.Controls.Add(actions, 0, 5);

            content.Controls.Add(layout);
            Controls.Add(content);
            Controls.Add(titleBar);

            AcceptButton = okButton;
            var history = GetReleaseHistory(version);
            var defaultIndex = history.FindIndex(release => release.Version == version);
            _versionSelector.SelectedIndex = defaultIndex >= 0 ? defaultIndex : 0;
            ResumeLayout(true);
        }

        private static List<ReleaseNotes> GetReleaseHistory(string currentVersion)
        {
            var releases = new List<ReleaseNotes>();
            foreach (var release in KnownReleases)
            {
                releases.Add(release);
            }

            var currentReleaseExists = releases.Exists(release => release.Version == currentVersion);
            if (!currentReleaseExists && !string.IsNullOrWhiteSpace(currentVersion))
            {
                releases.Insert(0, new ReleaseNotes(
                    currentVersion,
                    new[] { "WhatsNew.Form.NoNotes" },
                    new[] { "No release notes are available for this version yet." }));
            }

            return releases;
        }

        private static ReleaseNotes GetRelease(string version)
        {
            foreach (var release in KnownReleases)
            {
                if (release.Version == version) return release;
            }

            return new ReleaseNotes(
                version,
                new[] { "WhatsNew.Form.NoNotes" },
                new[] { "No release notes are available for this version yet." });
        }

        private void RefreshFeatureList(string version)
        {
            if (_featureList == null) return;

            var release = GetRelease(version);
            foreach (Control control in _featureList.Controls)
            {
                control.Dispose();
            }
            _featureList.Controls.Clear();
            _featureList.RowStyles.Clear();
            _featureList.RowCount = Math.Max(1, release.FeatureKeys.Length);
            for (var index = 0; index < release.FeatureKeys.Length; index++)
            {
                _featureList.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / release.FeatureKeys.Length));
                var feature = new Label
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 3, 0, 3),
                    Padding = new Padding(14, 0, 12, 0),
                    Text = "\u2022  " + GetLocalizedText(release.FeatureKeys[index], release.FeatureFallbacks[index]),
                    ForeColor = TextColor,
                    BackColor = FeatureColor,
                    Font = CreateUiFont(10f, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                _featureList.Controls.Add(feature, 0, index);
            }
        }

        private static Button CreateButton(string text)
        {
            var button = new Button
            {
                Dock = DockStyle.Fill,
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentColor,
                ForeColor = Color.White,
                Font = CreateUiFont(9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(6, 8, 0, 8)
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(67, 191, 234);
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
