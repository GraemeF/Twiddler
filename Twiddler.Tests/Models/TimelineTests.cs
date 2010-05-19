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
        [Fact]
        public void Tweets_WhenPollerPublishesATweet_PublishesATweet()
        {
            var stubSource = new Mock<ITweetSource>();
            var tweets = new Subject<ITweet>();
            stubSource.Setup(x => x.Tweets).Returns(tweets);

            var test = new Timeline(stubSource.Object );

            ITweet tweet = New.Tweet;
            tweets.OnNext(tweet);

            Assert.Contains(tweet,test.Tweets);
        }
    }
}