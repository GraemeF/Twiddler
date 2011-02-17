namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System.Collections.Generic;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Services;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class MemoryTweetStoreTests
    {
        [Fact]
        public void GetTweet_GivenAKnownTweet_ReturnsTheTweet()
        {
            var test = new MemoryTweetStore();

            ITweet tweet = A.Tweet.Build();
            test.Add(tweet);

            test.GetTweet(tweet.Id).Should().Be.SameAs(tweet);
        }

        [Fact]
        public void GetTweet_GivenAnUnknownTweet_Throws()
        {
            var test = new MemoryTweetStore();

            Assert.Throws(typeof(KeyNotFoundException), 
                          () => test.GetTweet("456"));
        }
    }
}