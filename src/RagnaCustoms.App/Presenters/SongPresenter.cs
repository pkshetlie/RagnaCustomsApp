using RagnaCustoms.Models;
using RagnaCustoms.Views;
using System.Threading.Tasks;

namespace RagnaCustoms.Presenters
{
    public class SongPresenter
    {
        protected virtual ISongView View { get; }
        protected virtual ISongProvider SongProvider { get; }

        public SongPresenter(ISongProvider songProvider)
        {
            SongProvider = songProvider;
        }
        public SongPresenter(ISongView view, ISongProvider songProvider) : this(songProvider)
        {
            View = view;
            View.Presenter = this;
        }

        public void SearchLocal(string term)
        {
            View.Songs = SongProvider.SearchLocal(term);
        }

        public async Task SearchOnlineAsync(string term)
        {
            View.Songs = await SongProvider.SearchOnlineAsync(term);
        }

        public async Task DownloadAsync(int songId)
        {
            await SongProvider.DownloadAsync(songId);
        }
    }
}
