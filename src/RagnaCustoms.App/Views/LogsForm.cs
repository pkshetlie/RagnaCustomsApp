using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Globalization;
using RagnaCustoms.App.Properties;
using RagnaCustoms.Models;

namespace RagnaCustoms.App.Views
{
    public partial class LogsForm : Form
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color BorderColor = Color.FromArgb(55, 80, 98);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);
        private static readonly Color AccentTextColor = Color.FromArgb(5, 22, 32);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public LogsForm()
        {
            InitializeComponent();
            Text = GetLocalizedText("Logs.Form.Title", "Logs");
            dateLogsDataGridViewTextBoxColumn.HeaderText = GetLocalizedText("Logs.Form.Date", "Date");
            songNameLogsDataGridViewTextBoxColumn.HeaderText = GetLocalizedText("Logs.Form.SongName", "Song name");
            difficultyLogsDataGridViewTextBoxColumn.HeaderText = GetLocalizedText("Logs.Form.Difficulty", "Difficulty");
            scoreLogsDataGridViewTextBoxColumn.HeaderText = GetLocalizedText("Logs.Form.Score", "Score");
            statusLogsDataGridViewTextBoxColumn.HeaderText = GetLocalizedText("Logs.Form.Status", "Status");
            hashLogsDataGridViewTextBoxColumn.HeaderText = GetLocalizedText("Logs.Form.Hash", "Hash");
            button1.Text = GetLocalizedText("Logs.Form.Refresh", "Refresh");
            NoLogsWarn.Text = GetLocalizedText("Logs.Form.NoLogs", "No logs to display");
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
            ClientSize = new Size(900, 620);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            MaximizeBox = false;
            MinimizeBox = false;

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

            var content = new Panel { Dock = DockStyle.Fill, BackColor = WindowColor, Padding = new Padding(22, 18, 22, 22) };
            Controls.Clear();
            Controls.Add(content);
            Controls.Add(titleBar);

            menuStrip1.Visible = false;
            button1.Location = new Point(22, 18);
            button1.Size = new Size(112, 34);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.BackColor = AccentColor;
            button1.ForeColor = AccentTextColor;
            button1.Font = CreateUiFont(8.5f, FontStyle.Bold);
            button1.Cursor = Cursors.Hand;
            button1.UseVisualStyleBackColor = false;

            NoLogsWarn.Location = new Point(152, 28);
            NoLogsWarn.ForeColor = MutedTextColor;
            NoLogsWarn.Font = CreateUiFont(9f, FontStyle.Regular);

            LogsDataGridView.Location = new Point(22, 68);
            LogsDataGridView.Size = new Size(856, 510);
            LogsDataGridView.BackgroundColor = SurfaceColor;
            LogsDataGridView.BorderStyle = BorderStyle.FixedSingle;
            LogsDataGridView.GridColor = BorderColor;
            LogsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            LogsDataGridView.RowHeadersVisible = false;
            LogsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LogsDataGridView.MultiSelect = false;
            LogsDataGridView.EnableHeadersVisualStyles = false;
            LogsDataGridView.ColumnHeadersHeight = 36;
            LogsDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = InputColor,
                ForeColor = AccentColor,
                SelectionBackColor = InputColor,
                SelectionForeColor = AccentColor,
                Font = CreateUiFont(8f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            LogsDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SurfaceColor,
                ForeColor = TextColor,
                SelectionBackColor = Color.FromArgb(31, 91, 116),
                SelectionForeColor = TextColor,
                Font = CreateUiFont(9f, FontStyle.Regular),
                Padding = new Padding(8, 0, 8, 0)
            };
            LogsDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = InputColor,
                ForeColor = TextColor,
                SelectionBackColor = Color.FromArgb(31, 91, 116),
                SelectionForeColor = TextColor
            };
            LogsDataGridView.RowTemplate.Height = 34;
            LogsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            content.Controls.Add(NoLogsWarn);
            content.Controls.Add(button1);
            content.Controls.Add(LogsDataGridView);
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

        public void Logs_Load(object sender, EventArgs e)
        {
            char[] delimiterChars = { ' ', ';', '\\' };

            //check if the file exist
            try
            {
                // Read each line of the file into a string array. Each element
                // of the array is one line of the file.
                var lines = File.ReadAllLines(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ragnacustom.log"));

                List<string> listFile = new();
                List<string> listSession = new();
                List<string> listUpload = new();

                //separate the 3 types of lines in 3 distincts arrays
                foreach (var line in lines)
                {
                    if (line.Contains("file")) listFile.Add(line);
                    if (line.Contains("session")) listSession.Add(line);
                    if (line.Contains("Upload")) listUpload.Add(line);
                }

                //check if songs have been uploaded (success or error)
                if (listUpload.Count > 0)
                    //recreate the object to display
                    foreach (var lineUpload in listUpload)
                    {
                        Logs logsToDisplay = new();
                        var splitedLineUpload = lineUpload.Split(delimiterChars);
                        var hashUpload = splitedLineUpload[7];
                        var dateUpload = splitedLineUpload[0] + " " + splitedLineUpload[1];
                        logsToDisplay.HashLogs = hashUpload;
                        logsToDisplay.ScoreLogs = splitedLineUpload[10];
                        logsToDisplay.DateLogs = dateUpload;
                        logsToDisplay.StatusLogs = splitedLineUpload[3] + " " + splitedLineUpload[4];


                        foreach (var lineSession in listSession)
                            if (lineSession.Contains(hashUpload))
                            {
                                var splitedLineSession = lineSession.Split(delimiterChars);

                                logsToDisplay.DifficultyLogs = splitedLineSession[10];
                                logsToDisplay.ScoreLogs = splitedLineSession[13];
                            }

                        foreach (var lineFile in listFile)
                            if (lineFile.Contains(hashUpload))
                            {
                                var splitedLineFile = lineFile.Split(delimiterChars);
                                logsToDisplay.SongNameLogs = splitedLineFile[16];
                            }

                        logsBindingSource.Insert(0, logsToDisplay);
                        noLogsWarning_Click(false, e);
                    }
                else
                    //if the file exist but does not contain upload lines
                    noLogsWarning_Click(true, e);
            }
            catch
            {
                //in case there is no file to read
                noLogsWarning_Click(true, e);
            }
        }

        private void RefreshLogs_Click(object sender, EventArgs e)
        {
            //clear the dgv for refreshing the logs
            LogsDataGridView.Rows.Clear();
            Logs_Load(sender, e);
        }

        private void noLogsWarning_Click(object sender, EventArgs e)
        {
            //display a message to the user if the file does not exist
            //or if no upload line is in the file
            if ((bool)sender)
                NoLogsWarn.Visible = true;
            else
                NoLogsWarn.Visible = false;
        }
    }
}
