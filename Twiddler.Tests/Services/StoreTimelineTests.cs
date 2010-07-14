using System;
using System.Collections.Generic;
using Moq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class StoreTimelineTests
    {
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void GettingTweets_Initially_IsEmpty()
        {
            StoreTimeline test = BuildDefaultTestSubject();

            Assert.Empty(test.Tweets);
        }

        [Fact]
        public void GettingTweets_WhenTheStoreHasInboxTweets_ContainsTweets()
        {
            ITweet tweet = A.Tweet.Build();
            StoreInboxTweetsChangesTo(new[] {tweet});

            StoreTimeline test = BuildDefaultTestSubject();

            Assert.Contains(tweet, test.Tweets);
        }

        [Fact]
        public void GettingTweets_WhenNewTweetsAreAddedToTheStore_ContainsNewTweets()
        {
            StoreTimeline test = BuildDefaultTestSubject();
            ITweet tweet = A.Tweet.Build();

            bool eventRaised = false;
            test.Tweets.CollectionChanged += (sender, args) => eventRaised = args.NewItems.Contains(tweet);
            StoreInboxTweetsChangesTo(new[] {tweet});

            Assert.True(eventRaised);
            Assert.Contains(tweet, test.Tweets);
        }

        [Fact]
        public void GettingTweets_WhenTheSameTweetsAreStillInTheStore_ContainsNoNewTweets()
        {
            const string TestId = "123";
            StoreTimeline test = BuildDefaultTestSubject();

            ITweet tweet = A.Tweet.IdentifiedBy(TestId).Build();
            StoreInboxTweetsChangesTo(new[] {tweet});

            bool eventRaised = false;
            test.Tweets.CollectionChanged += (sender, args) => eventRaised = true;

            StoreInboxTweetsChangesTo(new ITweet[] { A.Tweet.IdentifiedBy(TestId).Build() });

            Assert.False(eventRaised);
            Assert.Contains(tweet, test.Tweets);
        }

        [Fact]
        public void GettingTweets_WhenTweetsRemovedFromTheStore_DoesNotContainOldTweets()
        {
            StoreTimeline test = BuildDefaultTestSubject();
            ITweet tweet = A.Tweet.Build();
            StoreInboxTweetsChangesTo(new[] {tweet});

            bool eventRaised = false;
            test.Tweets.CollectionChanged += (sender, args) => eventRaised = args.OldItems.Contains(tweet);

            StoreInboxTweetsChangesTo(new ITweet[] {});
            Assert.True(eventRaised);
            Assert.DoesNotContain(tweet, test.Tweets);
        }

        private void StoreInboxTweetsChangesTo(IEnumerable<ITweet> inboxTweets)
        {
            _fakeStore.
                Setup(x => x.GetInboxTweets()).
                Returns(inboxTweets);
            _fakeStore.Raise(x => x.Updated += null, new EventArgs());
        }

        private StoreTimeline BuildDefaultTestSubject()
        {
            return new StoreTimeline(_fakeStore.Object);
        }
    }
}