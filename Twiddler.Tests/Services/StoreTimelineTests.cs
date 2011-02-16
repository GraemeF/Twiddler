namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System.Collections.Generic;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class StoreTimelineTests
    {
        private readonly ITweetStore _tweetStore = Substitute.For<ITweetStore>();

        [Fact]
        public void Add_GivenANewTweet_AddsTweetToTweets()
        {
            ITweet tweet = A.Tweet.Build();

            StoreTimeline test = BuildDefaultTestSubject();
            test.Add(tweet);

            test.Tweets.Should().Contain.Item(tweet);
        }

        [Fact]
        public void Add_GivenATweet_AddsTweetToStore()
        {
            ITweet tweet = A.Tweet.Build();

            StoreTimeline test = BuildDefaultTestSubject();
            test.Add(tweet);

            _tweetStore.Received().Add(tweet);
        }

        [Fact]
        public void Add_GivenAnUpdatedTweet_ReplacesTweetInTweets()
        {
            ITweet tweet = A.Tweet.Build();

            StoreTimeline test = BuildDefaultTestSubject();
            test.Add(tweet);

            test.Tweets.Should().Contain.Item(tweet);
        }

        [Fact]
        public void Add_GivenManyTweets_AddsTweetsToStore()
        {
            var tweets = new[] { A.Tweet.Build() };

            StoreTimeline test = BuildDefaultTestSubject();
            test.Add(tweets);

            _tweetStore.Received().Add(tweets);
        }

        [Fact]
        public void GettingTweets_Initially_IsEmpty()
        {
            StoreTimeline test = BuildDefaultTestSubject();

            test.Tweets.Should().Be.Empty();
        }

        [Fact]
        public void GettingTweets_WhenTheStoreHasInboxTweets_ContainsTweets()
        {
            ITweet tweet = A.Tweet.Build();
            StoreHasInboxTweets(new[] { tweet });

            StoreTimeline test = BuildDefaultTestSubject();

            test.Tweets.Should().Contain.Item(tweet);
        }

        private StoreTimeline BuildDefaultTestSubject()
        {
            return new StoreTimeline(_tweetStore);
        }

        private void StoreHasInboxTweets(IEnumerable<ITweet> inboxTweets)
        {
            _tweetStore.GetInboxTweets().Returns(inboxTweets);
        }
    }
}