using System;
using Twiddler.Core.Models;
using Twiddler.TwitterStore.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<Tweet> InboxTweets { get; }
    }
}