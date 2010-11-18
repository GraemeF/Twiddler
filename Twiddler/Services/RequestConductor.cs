using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (IRequestConductor))]
    [Export(typeof (IRequestConductor))]
    public class RequestConductor : IRequestConductor
    {
        private readonly ITwitterClient _client;
        private readonly INewTweetFilter _newTweetFilter;
        private readonly IEnumerable<ITweetRequester> _tweetRequesters;
        private PropertyObserver<ITwitterClient> _statusObserver;
        private IDisposable _subscription;
        private ITweetSink _tweetSink;

        [ImportingConstructor]
        public RequestConductor(ITwitterClient client,
                                [ImportMany] IEnumerable<ITweetRequester> tweetRequesters,
                                INewTweetFilter newTweetFilter)
        {
            _client = client;
            _tweetRequesters = tweetRequesters;
            _newTweetFilter = newTweetFilter;
        }

        #region IRequestConductor Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start(ITweetSink tweetSink)
        {
            _tweetSink = tweetSink;

            _statusObserver = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => PollIfAuthorized());
            PollIfAuthorized();
        }

        #endregion

        private void PollIfAuthorized()
        {
            if (_client.AuthorizationStatus == AuthorizationStatus.Authorized)
                EnsurePolling();
            else
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

        private void RequestAndAddNewTweetsToStore(ITweetRequester tweetRequester)
        {
            IEnumerable<ITweet> requestTweets = tweetRequester.RequestTweets();
            IEnumerable<ITweet> removeKnownTweets = _newTweetFilter.RemoveKnownTweets(requestTweets);
            _tweetSink.Add(removeKnownTweets);
        }

        ~RequestConductor()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                EnsureNotPolling();
        }
    }
}