using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagnaCustoms.Models
{
    public interface ISongProvider
    {
        Task DownloadAsync(int songId);
        IEnumerable<Song> SearchLocal();
        IEnumerable<Song> SearchLocal(string term);
        Task<IEnumerable<Song>> SearchOnlineAsync(string term);
    }
}
