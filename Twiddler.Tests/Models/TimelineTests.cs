using System.Collections.Generic;
using Moq;
using Twiddler.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Models
{
    public class TimelineTests
    {
        private readonly Mock<ITweetSource> _stubSource = new Mock<ITweetSource>();
        private readonly Subject<ITweet> _tweets = new Subject<ITweet>();

        public TimelineTests()
        {
            _stubSource.Setup(x => x.Tweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets_WhenPollerPublishesATweet_ContainsATweet()
        {
            Timeline test = BuildDefaultTestSubject();

            ITweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Contains(tweet, test.Tweets);
        }

        private Timeline BuildDefaultTestSubject()
        {
            return new Timeline(_stubSource.Object);
        }

        [Fact]
        public void Dispose__UnsubscribesFromNewTweets()
        {
            Timeline test = BuildDefaultTestSubject();
            test.Dispose();

            ITweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Empty(test.Tweets);
        }
    }
}