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
        public IEnumerable<Song> Songs
        {
            get => (IEnumerable<Song>)SearchResultGridView.DataSource;
            set => SearchResultGridView.DataSource = value;
        }

        public SongForm()
        {
            InitializeComponent();

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

        private async void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                await Presenter.SearchOnlineAsync(SearchTextBox.Text);
            }
        }

        private void SearchResultGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var song = (Song)senderGrid.Rows[e.RowIndex].DataBoundItem;

                Presenter.DownloadAsync(song.Id);
            }
        }
    }
}
