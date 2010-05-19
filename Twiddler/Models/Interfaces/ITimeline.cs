using System;
using System.Collections.ObjectModel;

namespace Twiddler.Models.Interfaces
{
    public interface ITimeline
    {
        ObservableCollection<ITweet> Tweets { get; }
    }
}