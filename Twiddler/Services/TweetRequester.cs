using System;
using System.Linq;
using Caliburn.Core.IoC;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("Public Timeline", typeof (ITweetRequester))]
    public class TweetRequester : ITweetRequester
    {
        private readonly ITwitterClient _client;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private readonly Factories.TweetFactory _tweetFactory;

        public TweetRequester(ITwitterClient client,
                              Factories.TweetFactory tweetFactory,
                              IRequestLimitStatus requestLimitStatus)
        {
            _client = client;
            _tweetFactory = tweetFactory;
            _requestLimitStatus = requestLimitStatus;
        }

        #region ITweetRequester Members

        public void Request()
        {
            CreateRequest().Request();
        }

        public event EventHandler<NewTweetsEventArgs> NewTweets = delegate { };

        #endregion

        private ITwitterPublicTimeline CreateRequest()
        {
            ITwitterPublicTimeline request = _client.
                MakeRequestFor().
                Statuses().
                OnPublicTimeline();

            request.CallbackTo(GotTweets);

            return request;
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