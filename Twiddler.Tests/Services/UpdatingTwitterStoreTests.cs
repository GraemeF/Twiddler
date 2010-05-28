using System;
using System.Collections.Generic;
using Moq;
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
        private readonly Subject<Tweet> _tweets = new Subject<Tweet>();

        [Fact]
        public void GettingTweets__ReturnsObservableTweetIdsFromSource()
        {
            Tweet tweet = New.Tweet;

            UpdatingTwitterStore test = BuildDefaultTestSubject();

            bool observed = false;
            test.NewTweets.Subscribe(x =>
                                         {
                                             Assert.Equal(tweet.Id, x);
                                             observed = true;
                                         });

            _tweets.OnNext(tweet);

            Assert.True(observed);
        }

        [Fact]
        public void PublishingSourceTweets__AddsTweetToStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();
            Tweet tweet = New.Tweet;

            _tweets.OnNext(tweet);
            GC.KeepAlive(test);

            _fakeStore.Verify(x => x.AddTweet(tweet));
        }

        [Fact]
        public void AddTweet__AddsTweetToStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeStore.Verify(x => x.AddTweet(tweet));
        }

        [Fact]
        public void GetTweet__GetsTweetFromStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            _fakeStore.Setup(x => x.GetTweet(tweet.Id)).Returns(tweet).Verifiable();

            Assert.Same(tweet, test.GetTweet(tweet.Id));

            _fakeStore.VerifyAll();
        }

        private UpdatingTwitterStore BuildDefaultTestSubject()
        {
            return new UpdatingTwitterStore(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}