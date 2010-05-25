using System;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface IUpdatingTweetStore : ITweetStore
    {
        IObservable<TweetId> NewTweets { get; }
    }
}