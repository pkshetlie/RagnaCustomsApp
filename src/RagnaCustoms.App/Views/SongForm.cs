using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace RagnaCustoms.Views
{
    public partial class SongForm : Form, ISongView
    {
        public SongPresenter Presenter { private get; set; }
        public IEnumerable<SongSearchModel> Songs
        {
            get => (IEnumerable<SongSearchModel>)SearchResultGridView.DataSource;
            set => SearchResultGridView.DataSource = value;
        }

        public SongForm()
        {
            InitializeComponent();

            SearchResultGridView.AutoGenerateColumns = false;

            Text += $" {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
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

                Presenter.DownloadAsync(song.Id);
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApiKeyMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ApiKey = Prompt.ShowDialog("Enter your API key :", "RagnaCustoms", Presenter.ApiKey);
        }

        private void SendScoreMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.SendScoreAutomatically = SendScoreAutomaticallyMenuItem.Checked;
        }
    }
}
