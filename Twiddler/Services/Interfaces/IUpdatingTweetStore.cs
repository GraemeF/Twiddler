using System;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface IUpdatingTweetStore : ITweetStore, ITweetSource
    {
        IObservable<TweetId> NewTweets { get; }
    }
}