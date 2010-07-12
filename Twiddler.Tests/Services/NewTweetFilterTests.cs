using System.Collections.Generic;
using Moq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class NewTweetFilterTests
    {
        private readonly Mock<ITweetStore> _fakeResolver = new Mock<ITweetStore>();

        [Fact]
        public void RemoveKnownTweets_GivenMixtureOfNewAndKnownTweets_ReturnsNewTweets()
        {
            NewTweetFilter test = BuildDefaultTestSubject();

            Tweet knownTweet = A.Tweet.IdentifiedBy("Known Id");
            Tweet newTweet = A.Tweet.IdentifiedBy("New Id");

            _fakeResolver.
                Setup(x => x.GetTweet(knownTweet.Id)).
                Returns(knownTweet);
            _fakeResolver.
                Setup(x => x.GetTweet(newTweet.Id)).
                Returns((Tweet) null);

            IEnumerable<Tweet> result = test.RemoveKnownTweets(new[] {newTweet, knownTweet});
            Assert.Contains(newTweet, result);
            Assert.DoesNotContain(knownTweet, result);
        }

        private NewTweetFilter BuildDefaultTestSubject()
        {
            return new NewTweetFilter(_fakeResolver.Object);
        }
    }
}