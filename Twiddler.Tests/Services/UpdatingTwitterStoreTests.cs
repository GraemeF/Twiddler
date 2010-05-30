using System;
using Moq;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class UpdatingTwitterStoreTests
    {
        private readonly Mock<IRequestConductor> _fakeRequestConductor = new Mock<IRequestConductor>();
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void AddTweet_GivenANewTweet_AddsTweetToStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            TwitterStatus tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeStore.Verify(x => x.AddTweet(tweet));
        }

        [Fact]
        public void AddTweet_GivenANewTweet_PublishesTweet()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();
            TweetId? publishedTweetId = null;
            test.Tweets.Subscribe(x => publishedTweetId = x);

            TwitterStatus tweet = New.Tweet;

            _fakeStore.
                Setup(x => x.AddTweet(tweet)).
                Returns(true);

            test.AddTweet(tweet);

            Assert.Equal(tweet.GetTweetId(), publishedTweetId);
        }

        [Fact]
        public void AddTweet_GivenATweetThatHasAlreadyBeenAdded_DoesNotPublishTweet()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            TweetId? publishedTweetId = null;
            test.Tweets.Subscribe(x => publishedTweetId = x);

            TwitterStatus tweet = New.Tweet;

            _fakeStore.
                Setup(x => x.AddTweet(tweet)).
                Returns(false);

            test.AddTweet(tweet);

            Assert.False(publishedTweetId.HasValue);
        }

        [Fact]
        public void GetTweet__GetsTweetFromStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            TwitterStatus tweet = New.Tweet;
            _fakeStore.Setup(x => x.GetTweet(tweet.GetTweetId())).Returns(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.GetTweetId()));
        }

        private UpdatingTwitterStore BuildDefaultTestSubject()
        {
            return new UpdatingTwitterStore(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}