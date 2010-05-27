using System;
using System.Linq;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetPoller))]
    public class TweetPoller : ITweetPoller
    {
        private readonly ITwitterClient _client;
        private readonly Factories.TweetFactory _tweetFactory;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private IFluentTwitter _request;
        private PropertyObserver<ITwitterClient> _statusObserver;

        public TweetPoller(ITwitterClient client, Factories.TweetFactory tweetFactory, IRequestLimitStatus requestLimitStatus)
        {
            _client = client;
            _tweetFactory = tweetFactory;
            _requestLimitStatus = requestLimitStatus;

            _statusObserver = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => PollIfAuthorized());
            PollIfAuthorized();
        }

        #region ITweetPoller Members

        public event EventHandler<NewTweetsEventArgs> NewTweets = delegate { };

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private IFluentTwitter CreateRequest()
        {
            IFluentTwitter request = _client.
                MakeRequestFor().
                Statuses().
                OnPublicTimeline().
                Configuration.
                UseRateLimiting(20.Percent()).
                RepeatEvery(25.Seconds());

            request.CallbackTo(GotTweets);

            return request;
        }

        private void PollIfAuthorized()
        {
            if (_client.AuthorizationStatus == AuthorizationStatus.Authorized)
                EnsurePolling();
            else
                EnsureNotPolling();
        }

        private void EnsureNotPolling()
        {
            if (_request != null)
            {
                _request.Cancel();
                _request = null;
            }
        }

        private void EnsurePolling()
        {
            if (_request == null)
            {
                _request = CreateRequest();
                _request.BeginRequest();
            }
        }

        ~TweetPoller()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                EnsureNotPolling();
        }

        private void GotTweets(object sender, TwitterResult result, object userstate)
        {
            UpdateLimit(result.RateLimitStatus);

            if (!result.SkippedDueToRateLimiting)
                NewTweets(this, new NewTweetsEventArgs(result.
                                                           AsStatuses().
                                                           Select(x => _tweetFactory(x))));
        }

        private void UpdateLimit(TwitterRateLimitStatus status)
        {
            _requestLimitStatus.HourlyLimit = status.HourlyLimit;
            _requestLimitStatus.PeriodEndTime = status.ResetTime;
            _requestLimitStatus.PeriodDuration = 1.Hour();
            _requestLimitStatus.RemainingHits = status.RemainingHits;
        }
    }
}