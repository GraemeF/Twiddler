using System;
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
            Tweet tweet = New.Tweet;
            StoreInboxTweetsChangesTo(new[] {tweet});

            StoreTimeline test = BuildDefaultTestSubject();

            Assert.Contains(tweet, test.Tweets);
        }

        [Fact]
        public void GettingTweets_WhenTheStoreIsUpdated_ContainsNewTweets()
        {
            StoreTimeline test = BuildDefaultTestSubject();
            Tweet tweet = New.Tweet;

            bool eventRaised = false;
            test.Tweets.CollectionChanged += (sender, args) => eventRaised = args.NewItems.Contains(tweet);
            StoreInboxTweetsChangesTo(new[] {tweet});

            Assert.True(eventRaised);
        }

        private void StoreInboxTweetsChangesTo(Tweet[] inboxTweets)
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