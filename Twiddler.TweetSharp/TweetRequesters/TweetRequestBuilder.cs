using System;
using System.Collections.Generic;
using System.Linq;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    public abstract class TweetRequestBuilder : ITweetRequester
    {
        protected readonly ITwitterClient Client;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private readonly Factories.TweetFactory _tweetFactory;

        private long _lastTweet;

        protected TweetRequestBuilder(ITwitterClient client,
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
            return GotTweets(CreateRequest(_lastTweet).GetResponse());
        }

        #endregion

        protected abstract ITwitterRequest CreateRequest(long since);

        private IEnumerable<ITweet> GotTweets(ITwitterResult result)
        {
            if (result.RateLimitStatus != null)
                UpdateLimit(result.RateLimitStatus);

            if (result.SkippedDueToRateLimiting ||
                result.IsNetworkError ||
                result.IsServiceError ||
                result.IsTwitterError)
                return new ITweet[] {};

            IEnumerable<IRawStatus> statuses = result.AsStatuses();

            _lastTweet = Math.Max(_lastTweet,
                                  statuses.Any()
                                      ? statuses.Max(x => x.Id)
                                      : 0L);

            return
                statuses.
                    Select(twitterStatus => _tweetFactory(twitterStatus));
        }

        private void UpdateLimit(IRateLimitStatus status)
        {
            _requestLimitStatus.HourlyLimit = status.HourlyLimit;
            _requestLimitStatus.PeriodEndTime = status.ResetTime;
            _requestLimitStatus.PeriodDuration = TimeSpan.FromHours(1);
            _requestLimitStatus.RemainingHits = status.RemainingHits;
        }
    }
}