using System;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface IUpdatingTweetStore
    {
        IObservable<Tweet> NewTweets { get; }
    }
}