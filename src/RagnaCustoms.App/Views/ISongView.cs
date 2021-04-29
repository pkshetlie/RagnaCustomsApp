using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using System.Collections.Generic;

namespace RagnaCustoms.Views
{
    public interface ISongView : IView
    {
        SongPresenter Presenter { set; }
        IEnumerable<Song> Songs { get; set; }
    }
}
