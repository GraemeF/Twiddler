using System;
using Twiddler.Core.Models;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<TweetId> Tweets { get; }
    }
}