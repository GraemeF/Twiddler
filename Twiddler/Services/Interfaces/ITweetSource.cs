using System;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<string> NewTweets { get; }
    }
}