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
    public class TwitterStoreUpdaterTests
    {
        private readonly Mock<IRequestConductor> _fakeRequestConductor = new Mock<IRequestConductor>();
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void AddTweet_GivenANewTweet_AddsTweetToStore()
        {
            TwitterStoreUpdater test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeStore.Verify(x => x.AddTweet(tweet));
        }

        [Fact]
        public void GetTweet__GetsTweetFromStore()
        {
            TwitterStoreUpdater test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            _fakeStore.Setup(x => x.GetTweet(tweet.Id)).Returns(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.Id));
        }

        private TwitterStoreUpdater BuildDefaultTestSubject()
        {
            return new TwitterStoreUpdater(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}