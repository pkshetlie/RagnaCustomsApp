namespace RagnaCustoms.Presenters
{
    public interface IDownloadingPresenter : IPresenter
    {
        void Download(int songId, bool autoClose,string songFolder=null, string subFolder = null);
    }
}