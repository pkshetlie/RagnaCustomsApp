using RagnaCustoms.Models;
using RagnaCustoms.Views;
using System.Threading.Tasks;

namespace RagnaCustoms.Presenters
{
    public class SongPresenter
    {
        protected virtual ISongView View { get; }
        protected virtual IDownloadingPresenter DownloadingPresenter { get; }
        protected virtual ISongProvider SongProvider { get; }

        public SongPresenter(ISongView view, IDownloadingPresenter downloadingPresenter, ISongProvider songProvider)
        {
            View = view;
            View.Presenter = this;
            DownloadingPresenter = downloadingPresenter;
            SongProvider = songProvider;
        }

        public void SearchLocal(string term)
        {
            View.Songs = SongProvider.SearchLocal(term);
        }

        public async Task SearchOnlineAsync(string term)
        {
            View.Songs = await SongProvider.SearchOnlineAsync(term);
        }

        public virtual void DownloadAsync(int songId)
        {
            DownloadingPresenter.Download(songId);
            DownloadingPresenter.ShowAsPopup();
        }
    }
}
