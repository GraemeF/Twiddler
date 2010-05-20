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
        readonly Mock<ITweetSource> _stubSource = new Mock<ITweetSource>();
        readonly Subject<ITweet> _tweets = new Subject<ITweet>();

        public TimelineTests()
        {
            _stubSource.Setup(x => x.Tweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets_WhenPollerPublishesATweet_ContainsATweet()
        {
            var test = new Timeline(_stubSource.Object );

            ITweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Contains(tweet,test.Tweets);
        }


        [Fact]
        public void Dispose__UnsubscribesFromNewTweets()
        {
            var test = new Timeline(_stubSource.Object);
            test.Dispose();

            ITweet tweet = New.Tweet;
            _tweets.OnNext(tweet);

            Assert.Empty(test.Tweets);
        }

    }
}