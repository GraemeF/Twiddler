using System;
using Twiddler.Core.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<Tweet> InboxTweets { get; }
    }
}