using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("Public Timeline", typeof (ITweetRequester))]
    public class PublicTimelineTweetRequester : ITweetRequester
    {
        private readonly ITwitterClient _client;
        private readonly IRequestLimitStatus _requestLimitStatus;
        private readonly Factories.TweetFactory _tweetFactory;
        private readonly Subject<Tweet> _tweets;

        public PublicTimelineTweetRequester(ITwitterClient client,
                                            Factories.TweetFactory tweetFactory,
                                            IRequestLimitStatus requestLimitStatus)
        {
            _client = client;
            _tweetFactory = tweetFactory;
            _requestLimitStatus = requestLimitStatus;

            _tweets = new Subject<Tweet>();
        }

        #region ITweetRequester Members

        public void Request()
        {
            CreateRequest().Request();
        }

        public IObservable<Tweet> Tweets
        {
            get { return _tweets; }
        }

        #endregion

        private ITwitterLeafNode CreateRequest()
        {
            return
                _client.
                    MakeRequestFor().
                    Statuses().
                    OnPublicTimeline().
                    CallbackTo(GotTweets);
        }

        private void GotTweets(object sender, TwitterResult result, object userstate)
        {
            UpdateLimit(result.RateLimitStatus);

            if (!result.SkippedDueToRateLimiting)
            {
                foreach (Tweet tweet in result.AsStatuses().Select(x => _tweetFactory(x)))
                    _tweets.OnNext(tweet);
            }
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