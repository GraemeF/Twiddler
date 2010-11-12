using System;
using Twiddler.TwitterStore.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<Tweet> InboxTweets { get; }
    }
}