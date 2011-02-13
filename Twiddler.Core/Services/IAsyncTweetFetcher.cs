namespace Twiddler.Core.Services
{
    #region Using Directives

    using System;

    #endregion

    public interface IAsyncTweetFetcher : IDisposable
    {
        void Start(ITweetSink tweetSink);
    }
}