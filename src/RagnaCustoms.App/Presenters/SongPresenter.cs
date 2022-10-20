using System.Configuration;
using System.Threading.Tasks;
using RagnaCustoms.Models;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using Configuration = RagnaCustoms.Services.Configuration;

namespace RagnaCustoms.Presenters
{
    public class SongPresenter
    {
        public SongPresenter(ISongView view, IDownloadingPresenter downloadingPresenter,
            ISongProvider songProvider)
        {
            _configuration = new Configuration();
            View = view;
            View.Presenter = this;           
            DownloadingPresenter = downloadingPresenter;
            SongProvider = songProvider;
            if (string.IsNullOrEmpty(_configuration.RequestFolder)){
                _configuration.RequestFolder = "Requests";
            }

            if (string.IsNullOrEmpty(_configuration.BaseFolder))
            {
                _configuration.BaseFolder = DirProvider.getCustomDirectory().FullName;
            }
        }

        protected virtual Configuration _configuration { get; }
        protected virtual ISongView View { get; }
        protected virtual IDownloadingPresenter DownloadingPresenter { get; }
        protected virtual ISongProvider SongProvider { get; }

        public virtual string ApiKey
        {
            get => _configuration.ApiKey;
            set => _configuration.ApiKey = value;
        }

        public string Lang
        {
            get => _configuration.Lang;
            set => _configuration.Lang = value;
        }

 
        public async Task SearchOnlineAsync(string term)
        {
            View.Songs = await SongProvider.SearchOnlineAsync(term);
        }

        public virtual void DownloadAsync(int songId, string songFolder= null, string subFolder = null)
        {
            DownloadingPresenter.Download(songId, _configuration.AutoCloseDownload, songFolder, subFolder);
            DownloadingPresenter.ShowAsPopup();
        }



        internal async Task CompareSongsAsync()
        {
            View.Songs = await SongProvider.CompareSongsWithOnlineAsync();
        }
    }
}