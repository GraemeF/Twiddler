using System.Collections.Generic;
using Moq;
using Twiddler.Core.Models;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class SubscribingTimelineTests
    {
        private readonly Mock<ISelfUpdatingTweetStore> _stubStore = new Mock<ISelfUpdatingTweetStore>();
        private readonly Subject<Tweet> _tweets = new Subject<Tweet>();

        public SubscribingTimelineTests()
        {
            _stubStore.Setup(x => x.InboxTweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets_WhenPollerPublishesATweet_ContainsATweet()
        {
            SubscribingTimeline test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Contains(tweet, test.Tweets);
        }

        private SubscribingTimeline BuildDefaultTestSubject()
        {
            return new SubscribingTimeline(_stubStore.Object);
        }

        [Fact]
        public void Dispose__UnsubscribesFromNewTweets()
        {
            SubscribingTimeline test = BuildDefaultTestSubject();
            test.Dispose();

            Tweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Empty(test.Tweets);
        }
    }
}