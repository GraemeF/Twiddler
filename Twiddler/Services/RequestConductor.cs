namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ReactiveUI;

    using Twiddler.Core.Services;

    #endregion

    public class RequestConductor : IAsyncTweetFetcher
    {
        private readonly IAuthorizer _client;

        private readonly IEnumerable<ITweetRequester> _tweetRequesters;

        private IDisposable _authorizationSubscription;

        private IDisposable _subscription;

        private ITweetSink _tweetSink;

        public RequestConductor(IAuthorizer client, 
                                IEnumerable<ITweetRequester> tweetRequesters)
        {
            _client = client;
            _tweetRequesters = tweetRequesters;
        }

        ~RequestConductor()
        {
            Dispose(false);
        }

        #region IAsyncTweetFetcher members

        public void Start(ITweetSink tweetSink)
        {
            _tweetSink = tweetSink;

            _authorizationSubscription = _client.
                WhenAny(x => x.AuthorizationStatus, _ => true).
                Subscribe(_ => PollIfAuthorized());

            PollIfAuthorized();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

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
            if (_subscription == null)
                _subscription = Observable.
                    Interval(TimeSpan.FromMinutes(1)).
                    StartWith(0L).
                    Subscribe(x => new TaskFactory().
                                       StartNew(() => Parallel.ForEach(_tweetRequesters, RequestAndAddNewTweetsToStore)));
        }

        private void PollIfAuthorized()
        {
            if (_client.AuthorizationStatus ==
                AuthorizationStatus.Authorized)
                EnsurePolling();
            else
                EnsureNotPolling();
        }

        private void RequestAndAddNewTweetsToStore(ITweetRequester tweetRequester)
        {
            _tweetSink.Add(tweetRequester.RequestTweets());
        }
    }
}