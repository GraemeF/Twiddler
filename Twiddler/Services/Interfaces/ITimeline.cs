using System.Collections.ObjectModel;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Services.Interfaces
{
    public interface ITimeline : ITweetSink
    {
        ObservableCollection<ITweet> Tweets { get; }
    }
}