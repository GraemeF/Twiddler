namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System.Collections.ObjectModel;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    public interface ITimeline : ITweetSink
    {
        ObservableCollection<ITweet> Tweets { get; }
    }
}