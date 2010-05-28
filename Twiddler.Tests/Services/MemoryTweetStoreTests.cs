﻿using System.Collections.Generic;
using Twiddler.Models;
using Twiddler.Services;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class MemoryTweetStoreTests
    {
        [Fact]
        public void AddTweet_GivenADuplicatedTweet_UpdatesStoredTweet()
        {
            var test = new MemoryTweetStore();

            test.AddTweet(New.Tweet);

            Tweet updatedTweet = New.Tweet;
            test.AddTweet(updatedTweet);

            Assert.Same(updatedTweet, test.GetTweet(updatedTweet.Id));
        }

        [Fact]
        public void GetTweet_GivenAKnownTweet_ReturnsTheTweet()
        {
            var test = new MemoryTweetStore();

            Tweet tweet = New.Tweet;
            test.AddTweet(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.Id));
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