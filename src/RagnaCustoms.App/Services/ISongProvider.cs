using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagnaCustoms.Models
{
    public interface ISongProvider
    {
        void DownloadAsync(int songId, Action<int> downloadProgressChanged, Action<bool> downloadCompleted, bool autoClose);

        IEnumerable<Song> SearchLocal();
        IEnumerable<Song> SearchLocal(string term);
        Task<IEnumerable<SongSearchModel>> SearchOnlineAsync(string term);
    }
}
