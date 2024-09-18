namespace RagnaCustoms.Presenters
{
    public interface IDownloadingPresenter : IPresenter
    {
        void Download(string songId, bool autoClose,string songFolder=null, string subFolder = null);
    }
}