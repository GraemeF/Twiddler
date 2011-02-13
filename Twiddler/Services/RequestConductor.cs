namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Caliburn.Core.IoC;

    using MvvmFoundation.Wpf;

    using Twiddler.Core.Services;
    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(IAsyncTweetFetcher))]
    [Export(typeof(IAsyncTweetFetcher))]
    public class RequestConductor : IAsyncTweetFetcher
    {
        private readonly IAuthorizer _client;

        private readonly IEnumerable<ITweetRequester> _tweetRequesters;

        private PropertyObserver<IAuthorizer> _statusObserver;

        private IDisposable _subscription;

        private ITweetSink _tweetSink;

        [ImportingConstructor]
        public RequestConductor(IAuthorizer client, 
                                [ImportMany] IEnumerable<ITweetRequester> tweetRequesters)
        {
            _client = client;
            _tweetRequesters = tweetRequesters;
        }

        ~RequestConductor()
        {
            Dispose(false);
        }

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IRequestConductor members

        public void Start(ITweetSink tweetSink)
        {
            _tweetSink = tweetSink;

            _statusObserver = new PropertyObserver<IAuthorizer>(_client).
                RegisterHandler(x => x.AuthorizationStatus, 
                                y => PollIfAuthorized());
            PollIfAuthorized();
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