namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System;

    using Twiddler.TwitterStore.Models;

    #endregion

    public interface ITweetSource
    {
        IObservable<Tweet> InboxTweets { get; }
    }
}