using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (IRequestConductor))]
    public class RequestConductor : IRequestConductor
    {
        private readonly ITwitterClient _client;
        private readonly IEnumerable<ITweetRequester> _tweetRequesters;
        private readonly ITweetSink _tweetSink;
        private PropertyObserver<ITwitterClient> _statusObserver;

        public RequestConductor(ITwitterClient client, IEnumerable<ITweetRequester> tweetRequesters,
                                ITweetSink tweetSink)
        {
            _client = client;
            _tweetRequesters = tweetRequesters;
            _tweetSink = tweetSink;

            _statusObserver = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => PollIfAuthorized());
            PollIfAuthorized();
        }

        public IObservable<Tweet> Tweets { get; private set; }

        #region IRequestConductor Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
        }

        private void EnsurePolling()
        {
            foreach (ITweetRequester requester in _tweetRequesters)
            {
                foreach (Tweet tweet in requester.RequestTweets())
                    _tweetSink.AddTweet(tweet);
            }
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