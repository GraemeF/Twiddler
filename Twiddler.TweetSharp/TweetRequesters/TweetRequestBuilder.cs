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
        protected readonly ITwitterClient Client;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private readonly Factories.TweetFactory _tweetFactory;

        private long _lastTweet;

        protected TweetRequester(ITwitterClient client,
                                 IRequestLimitStatus requestLimitStatus,
                                 Factories.TweetFactory tweetFactory)
        {
            Client = client;
            _requestLimitStatus = requestLimitStatus;
            _tweetFactory = tweetFactory;
        }

        #region ITweetRequester Members

        public IEnumerable<ITweet> RequestTweets()
        {
            return GotTweets(GetStatuses(_lastTweet));
        }

        #endregion

        protected abstract IEnumerable<TwitterStatus> GetStatuses(long since);

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