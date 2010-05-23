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
        private readonly Subject<Tweet> _tweets = new Subject<Tweet>();

        public TimelineTests()
        {
            _stubSource.Setup(x => x.Tweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets_WhenPollerPublishesATweet_ContainsATweet()
        {
            Timeline test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
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

            Tweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Empty(test.Tweets);
        }
    }
}