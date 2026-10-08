namespace RagnaCustoms.Presenters
{
    public interface IDownloadingPresenter : IPresenter
    {
        void Download(string songId, bool autoClose,string songFolder=null, string subFolder = null);
        void DownloadPlaylist(int playlistId, bool autoClose);
        void DownloadCompetition(int competitionId, bool autoClose);
        void DownloadSongs(System.Collections.Generic.IEnumerable<string> songIds, bool autoClose);
    }
}
