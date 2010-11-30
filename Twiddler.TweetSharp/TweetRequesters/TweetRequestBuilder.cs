using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp;
using Twiddler.Core.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    public abstract class TweetRequester : ITweetRequester
    {
        protected readonly ITwitterClientFactory ClientFactory;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private readonly Factories.TweetFactory _tweetFactory;

        private long _lastTweet;

        protected TweetRequester(ITwitterClientFactory clientFactory,
                                 IRequestLimitStatus requestLimitStatus,
                                 Factories.TweetFactory tweetFactory)
        {
            ClientFactory = clientFactory;
            _requestLimitStatus = requestLimitStatus;
            _tweetFactory = tweetFactory;
        }

        #region ITweetRequester Members

        public IEnumerable<ITweet> RequestTweets()
        {
            TwitterService service = ClientFactory.CreateService();

            IEnumerable<TwitterStatus> statuses = GetStatuses(service, _lastTweet);

            if (service.Response.RateLimitStatus != null)
                UpdateLimit(service.Response.RateLimitStatus);

            return GotTweets(statuses);
        }

        #endregion

        protected abstract IEnumerable<TwitterStatus> GetStatuses(TwitterService service, long since);

        private IEnumerable<ITweet> GotTweets(IEnumerable<TwitterStatus> statuses)
        {
            _lastTweet = Math.Max(_lastTweet,
                                  statuses.Any()
                                      ? statuses.Max(x => x.Id)
                                      : 0L);

            return
                statuses.
                    Select(twitterStatus => _tweetFactory(twitterStatus));
        }

        private void UpdateLimit(TwitterRateLimitStatus status)
        {
            _requestLimitStatus.HourlyLimit = status.HourlyLimit;
            _requestLimitStatus.PeriodEndTime = status.ResetTime;
            _requestLimitStatus.PeriodDuration = TimeSpan.FromHours(1);
            _requestLimitStatus.RemainingHits = status.RemainingHits;
        }
    }
}