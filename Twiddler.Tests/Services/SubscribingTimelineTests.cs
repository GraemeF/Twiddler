using System.Collections.Generic;
using Moq;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class SubscribingTimelineTests
    {
        private readonly Mock<ISelfUpdatingTweetStore> _stubStore = new Mock<ISelfUpdatingTweetStore>();
        private readonly Subject<string> _tweets = new Subject<string>();

        public SubscribingTimelineTests()
        {
            _stubStore.Setup(x => x.InboxTweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets_WhenPollerPublishesATweet_ContainsATweet()
        {
            SubscribingTimeline test = BuildDefaultTestSubject();

            string tweet = "5";
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

            string tweet = "5";
            _tweets.OnNext(tweet);

            Assert.Empty(test.Tweets);
        }
    }
}