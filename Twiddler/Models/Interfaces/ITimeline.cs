using System;

namespace Twiddler.Models.Interfaces
{
    public interface ITimeline
    {
        IObservable<ITweet> Tweets { get; }
    }
}