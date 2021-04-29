using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagnaCustoms.Models
{
    public interface ISongProvider
    {
        void DownloadAsync(int songId, Action<int> downloadProgressChanged, Action downloadCompleted);

        IEnumerable<Song> SearchLocal();
        IEnumerable<Song> SearchLocal(string term);
        Task<IEnumerable<Song>> SearchOnlineAsync(string term);
    }
}
