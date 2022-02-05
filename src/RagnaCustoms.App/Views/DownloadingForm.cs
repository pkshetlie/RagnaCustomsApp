using System;
using System.Windows.Forms;
using RagnaCustoms.Presenters;

namespace RagnaCustoms.Views
{
    public partial class DownloadingForm : Form, IDownloadingView
    {
        public DownloadingForm()
        {
            InitializeComponent();
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
            set => Text = value;
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