using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using System.Collections.Generic;

namespace RagnaCustoms.Views
{
    public interface ISongView : IView
    {
        SongPresenter Presenter { set; }
        IEnumerable<SongSearchModel> Songs { get; set; }

        bool SendScoreAutomatically { get; set; }
    }
}
