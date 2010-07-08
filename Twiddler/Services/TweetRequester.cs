using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
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

        public IEnumerable<Tweet> RequestTweets()
        {
            return GotTweets(CreateRequest(_lastTweet).Request());
        }

        #endregion

        protected abstract ITwitterLeafNode CreateRequest(long since);

        private IEnumerable<Tweet> GotTweets(TwitterResult result)
        {
            if (result.RateLimitStatus != null)
                UpdateLimit(result.RateLimitStatus);

            if (result.SkippedDueToRateLimiting ||
                result.IsNetworkError ||
                result.IsServiceError ||
                result.IsTwitterError)
                return new Tweet[] {};

            IEnumerable<TwitterStatus> statuses = result.AsStatuses();

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
            _requestLimitStatus.PeriodDuration = 1.Hour();
            _requestLimitStatus.RemainingHits = status.RemainingHits;
        }
    }
}