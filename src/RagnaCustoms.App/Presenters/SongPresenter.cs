using RagnaCustoms.Models;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using System.Threading.Tasks;

namespace RagnaCustoms.Presenters
{
    public class SongPresenter
    {
        protected virtual Configuration Configuration { get; }
        protected virtual ISongView View { get; }
        protected virtual IDownloadingPresenter DownloadingPresenter { get; }
        protected virtual ISongProvider SongProvider { get; }

        public virtual string ApiKey
        {
            get => Configuration.ApiKey;
            set => Configuration.ApiKey = value;
        }

        public virtual bool SendScoreAutomatically
        {
            get => Configuration.SendScoreAutomatically;
            set => Configuration.SendScoreAutomatically = value;
        }

        public SongPresenter(Configuration configuration, ISongView view, IDownloadingPresenter downloadingPresenter, ISongProvider songProvider)
        {
            Configuration = configuration;
            View = view;
            View.Presenter = this;
            DownloadingPresenter = downloadingPresenter;
            SongProvider = songProvider;
        }

        public void SearchLocal(string term)
        {
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

        public virtual void EnableSendScoreAutomatically()
        {
            Configuration.SendScoreAutomatically = true;
        }
        public virtual void DisableSendScoreAutomatically()
        {
            Configuration.SendScoreAutomatically = true;
        }

        public virtual string GetApiKey()
        {
            return Configuration.ApiKey;
        }
        public virtual void SetApiKey(string apiKey)
        {
            Configuration.ApiKey = apiKey;
        }
    }
}
