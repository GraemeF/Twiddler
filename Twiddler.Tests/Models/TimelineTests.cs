using System.Collections.Generic;
using Moq;
using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Models
{
    public class TimelineTests
    {
        private readonly Mock<IUpdatingTweetStore> _stubStore = new Mock<IUpdatingTweetStore>();
        private readonly Subject<TweetId> _tweets = new Subject<TweetId>();

        public TimelineTests()
        {
            _stubStore.Setup(x => x.Tweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets_WhenPollerPublishesATweet_ContainsATweet()
        {
            Timeline test = BuildDefaultTestSubject();

            var tweet = new TweetId(5);
            _tweets.OnNext(tweet);

            Assert.Contains(tweet, test.Tweets);
        }

        private Timeline BuildDefaultTestSubject()
        {
            return new Timeline(_stubStore.Object);
        }

        [Fact]
        public void Dispose__UnsubscribesFromNewTweets()
        {
            Timeline test = BuildDefaultTestSubject();
            test.Dispose();

            var tweet = new TweetId(5);
            _tweets.OnNext(tweet);

            Assert.Empty(test.Tweets);
        }
    }
}