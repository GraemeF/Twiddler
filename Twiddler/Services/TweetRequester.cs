using System.Collections.Generic;
using System.Linq;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    public abstract class TweetRequester : ITweetRequester
    {
        protected readonly ITwitterClient Client;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private readonly Factories.TweetFactory _tweetFactory;

        protected TweetRequester(ITwitterClient client,
                                 Factories.TweetFactory tweetFactory,
                                 IRequestLimitStatus requestLimitStatus)
        {
            Client = client;
            _tweetFactory = tweetFactory;
            _requestLimitStatus = requestLimitStatus;
        }

        #region ITweetRequester Members

        public IEnumerable<TwitterStatus> RequestTweets()
        {
            return GotTweets(CreateRequest().Request());
        }

        #endregion

        protected abstract ITwitterLeafNode CreateRequest();

        private IEnumerable<TwitterStatus> GotTweets(TwitterResult result)
        {
            if (result.RateLimitStatus != null)
                UpdateLimit(result.RateLimitStatus);

            if (result.SkippedDueToRateLimiting)
                return new TwitterStatus[] {};

            return
                result.
                    AsStatuses().
                    Select(x => _tweetFactory(x));
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