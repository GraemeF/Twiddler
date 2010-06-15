using System;
using Moq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class SelfUpdatingTwitterStoreTests
    {
        private readonly Mock<IRequestConductor> _fakeRequestConductor = new Mock<IRequestConductor>();
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void AddTweet_GivenANewTweet_AddsTweetToStore()
        {
            SelfUpdatingTwitterStore test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeStore.Verify(x => x.AddTweet(tweet));
        }

        [Fact]
        public void AddTweet_GivenANewTweet_PublishesTweet()
        {
            SelfUpdatingTwitterStore test = BuildDefaultTestSubject();
            string publishedTweetId = null;
            test.InboxTweets.Subscribe(x => publishedTweetId = x);

            Tweet tweet = New.Tweet;

            _fakeStore.
                Setup(x => x.AddTweet(tweet)).
                Returns(true);

            test.AddTweet(tweet);

            Assert.Equal(tweet.Id, publishedTweetId);
        }

        [Fact]
        public void AddTweet_GivenATweetThatHasAlreadyBeenAdded_DoesNotPublishTweet()
        {
            SelfUpdatingTwitterStore test = BuildDefaultTestSubject();

            string publishedTweetId = null;
            test.InboxTweets.Subscribe(x => publishedTweetId = x);

            Tweet tweet = New.Tweet;

            _fakeStore.
                Setup(x => x.AddTweet(tweet)).
                Returns(false);

            test.AddTweet(tweet);

            Assert.Null(publishedTweetId);
        }

        [Fact]
        public void GetTweet__GetsTweetFromStore()
        {
            SelfUpdatingTwitterStore test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            _fakeStore.Setup(x => x.GetTweet(tweet.Id)).Returns(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.Id));
        }

        private SelfUpdatingTwitterStore BuildDefaultTestSubject()
        {
            return new SelfUpdatingTwitterStore(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}