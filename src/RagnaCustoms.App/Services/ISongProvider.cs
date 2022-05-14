using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagnaCustoms.Models
{
    public interface ISongProvider
    {
        Task DownloadAsync(int songId, Action<int> downloadProgressChanged, Action<bool> downloadCompleted,
            Action<string> downloadTitle, bool autoClose, string songFolder);
           Task DownloadListAsync(int listId, Action<int> downloadProgressChanged, Action<bool> downloadCompleted,
            Action<string> downloadTitle, bool autoClose);

        IEnumerable<Song> SearchLocal();
        IEnumerable<Song> SearchLocal(string term);
        Task<IEnumerable<SongSearchModel>> SearchOnlineAsync(string term);
        Task<SongSearchModel> SearchOnlineAsync(int id);
        Task<IEnumerable<SongSearchModel>> CompareSongsWithOnlineAsync(string dir = null);
    }
}