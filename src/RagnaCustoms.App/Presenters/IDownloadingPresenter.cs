namespace RagnaCustoms.Presenters
{
    public interface IDownloadingPresenter : IPresenter
    {
        void Download(int songId, bool autoClose);
    }
}