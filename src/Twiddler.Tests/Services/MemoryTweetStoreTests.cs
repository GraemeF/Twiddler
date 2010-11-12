﻿using System.Collections.Generic;
using Twiddler.Core.Models;
using Twiddler.Services;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class MemoryTweetStoreTests
    {
        [Fact]
        public void GetTweet_GivenAKnownTweet_ReturnsTheTweet()
        {
            var test = new MemoryTweetStore();

            ITweet tweet = A.Tweet.Build();
            test.Add(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.Id));
        }

        [Fact]
        public void GetTweet_GivenAnUnknownTweet_Throws()
        {
            var test = new MemoryTweetStore();

            Assert.Throws(typeof (KeyNotFoundException),
                          () => test.GetTweet("456"));
        }
    }
}