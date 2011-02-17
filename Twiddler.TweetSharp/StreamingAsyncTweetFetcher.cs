namespace Twiddler.TweetSharp
{
    #region Using Directives

    using System;
    using System.Linq;

    using Twiddler.Core.Services;

    #endregion

    public class StreamingAsyncTweetFetcher // : IAsyncTweetFetcher

    {
        private readonly IAuthorizer _client;

        private IDisposable _statusSubscription;

        private IDisposable _subscription;

        private ITweetSink _tweetSink;

        public StreamingAsyncTweetFetcher(IAuthorizer client)
        {
            _client = client;
        }

        ~StreamingAsyncTweetFetcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start(ITweetSink tweetSink)
        {
            _tweetSink = tweetSink;
            _statusSubscription =
                _client.Changed.
                    Where(x => x.PropertyName == "AuthorizationStatus").
                    Select(x => x.Value).
                    StartWith(_client.AuthorizationStatus).
                    Subscribe(x => PollIfAuthorized());

            PollIfAuthorized();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                EnsureNotPolling();
        }

        private void EnsureNotPolling()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }
        }

        private void EnsurePolling()
        {
        }

        private void PollIfAuthorized()
        {
            if (_client.AuthorizationStatus ==
                AuthorizationStatus.Authorized)
                EnsurePolling();
            else
                EnsureNotPolling();
        }
    }
}