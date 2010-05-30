using System.Collections.Generic;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Services;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class MemoryTweetStoreTests
    {
        [Fact]
        public void AddTweet_GivenANewTweet_ReturnsTrue()
        {
            var test = new MemoryTweetStore();

            Assert.True(test.AddTweet(New.Tweet));
        }

        [Fact]
        public void AddTweet_GivenADuplicatedTweet_ReturnsFalse()
        {
            var test = new MemoryTweetStore();

            test.AddTweet(New.Tweet);

            Assert.False(test.AddTweet(New.Tweet));
        }

        [Fact]
        public void GetTweet_GivenAKnownTweet_ReturnsTheTweet()
        {
            var test = new MemoryTweetStore();

            TwitterStatus tweet = New.Tweet;
            test.AddTweet(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.GetTweetId()));
        }

        [Fact]
        public void GetTweet_GivenAnUnknownTweet_Throws()
        {
            var test = new MemoryTweetStore();

            Assert.Throws(typeof (KeyNotFoundException),
                          () => test.GetTweet(new TweetId(456)));
        }
    }
}