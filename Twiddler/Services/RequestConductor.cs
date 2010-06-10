using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using TweetSharp.Extensions;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (IRequestConductor))]
    public class RequestConductor : IRequestConductor
    {
        private readonly ITwitterClient _client;
        private readonly IEnumerable<ITweetRequester> _tweetRequesters;
        private PropertyObserver<ITwitterClient> _statusObserver;
        private IDisposable _subscription;
        private ITweetSink _tweetSink;

        public RequestConductor(ITwitterClient client, IEnumerable<ITweetRequester> tweetRequesters)
        {
            _client = client;
            _tweetRequesters = tweetRequesters;
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
                    Interval(1.Minute()).
                    StartWith(0L).
                    Subscribe(x => new TaskFactory().
                                       StartNew(() => Parallel.ForEach(_tweetRequesters, MakeRequest)));
        }

        private void MakeRequest(ITweetRequester tweetRequester)
        {
            foreach (Tweet tweet in tweetRequester.RequestTweets())
            {
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