using System;
using TweetSharp.Twitter.Model;
using Twiddler.Models.Interfaces;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<ITweet> Tweets { get; }
    }
}