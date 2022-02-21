using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using RagnaCustoms.App;
using RagnaCustoms.App.Extensions;
using RagnaCustoms.App.Properties;
using RagnaCustoms.App.Views;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;

namespace RagnaCustoms.Views
{
    public partial class SongForm : Form, ISongView
    {
        public SongForm()
        {
            InitializeComponent();
            SearchResultGridView.AutoGenerateColumns = false;
            var conf = new Configuration();
            englishToolStripMenuItem.Checked = conf.Lang == "en";
            frenchToolStripMenuItem.Checked = conf.Lang == "fr";
            Text += $" {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
        }

        public SongPresenter Presenter { private get; set; }

        public IEnumerable<SongSearchModel> Songs
        {
            get => (IEnumerable<SongSearchModel>)SearchResultGridView.DataSource;
            set => SearchResultGridView.DataSource = value;
        }

        public bool SendScoreAutomatically
        {
            get => SendScoreAutomaticallyMenuItem.Checked;
            set => SendScoreAutomaticallyMenuItem.Checked = value;
        }

        public bool AutoCloseDownload
        {
            get => autoCloseDownloadToolStripMenuItem.Checked;
            set => autoCloseDownloadToolStripMenuItem.Checked = value;
        }

        public bool Overlay
        {
            get => overlayToolStripMenuItem.Checked;
            set => overlayToolStripMenuItem.Checked = value;
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
                    senderGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Aquamarine;
                    Presenter.DownloadAsync(song.Id);
                }
                else if (senderGrid.Columns[e.ColumnIndex].DataPropertyName == "Delete")
                {
                    try
                    {
                        var songDirectoryPath = Path.Combine(DirProvider.RagnarockSongDirectoryPath, $"{song.Name.Slug()}{song.Author.Slug()}{song.Mapper.Slug()}");
                        Directory.Delete(songDirectoryPath, true);
                        //var songs = new List<SongSearchModel>();                        
                        //for(var i = 0; i<senderGrid.Rows.Count ; i++)
                        //{
                        //    var thisSong = (SongSearchModel)senderGrid.Rows[i].DataBoundItem;
                        //    if (song.Id != thisSong.Id)
                        //    {
                        //        songs.Add(song);
                        //    }
                        //}
                        //senderGrid.Rows.Clear();
                        //senderGrid.Rows.Add(songs);
                        
                        senderGrid.Rows.Remove(senderGrid.Rows[e.RowIndex]);


                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("We can't delete this one for you please go to the customs songs directory and delete it manually");
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

        private void SendScoreMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.SendScoreAutomatically = SendScoreAutomaticallyMenuItem.Checked;
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

        private void autoCloseDownloadToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.AutoCloseDownload = autoCloseDownloadToolStripMenuItem.Checked;
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
            var device = Oculus.GetDevice();
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
            var result = Oculus.SyncSongs();
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
            new TwitchBotForm().Show();
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
                var sInfo = new ProcessStartInfo($"https://ragnacustoms.com/overlay/display/{Presenter.ApiKey}");
                Process.Start(sInfo);
            }
        }

        private void overlayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.Overlay = overlayToolStripMenuItem.Checked;
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
            MessageBox.Show(this, "Please restart application to apply language", "Restart needed",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void frenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.Lang = "fr";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Presenter.Lang, true);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            MessageBox.Show(this, "Veuillez redemarrer l'application pour appliquer la nouvelle langue",
                "Redemarrage nécessaire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SearchResultGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            foreach (DataGridViewRow row in senderGrid.Rows)
            {
                var song = (SongSearchModel)row.DataBoundItem;
                row.DefaultCellStyle.BackColor =
                    song.UpToDate ? Color.Aquamarine : Color.DarkSalmon;
                row.Cells[row.Cells.Count - 1].Dispose();
            }
        }

        private void sendScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = Oculus.SendScore();
            if (result == 0)
                MessageBox.Show(Resources.Song_Form_SyncComplete, "RagnaCustoms", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else if (result == 1)
                MessageBox.Show(Resources.Song_Form_NoCompatibleDeviceFound, "RagnaCustoms", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var dir = DirProvider.getCustomDirectory().FullName;
            Process.Start("explorer.exe", dir);
        }
    }
}