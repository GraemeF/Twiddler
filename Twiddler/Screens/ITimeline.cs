using System;

namespace Twiddler.Screens
{
    public interface ITimeline
    {
        IObservable<ITweet> Tweets { get; }
    }
}