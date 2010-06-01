using System.Threading;
using Moq;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class LoadingTweetScreenTests
    {
        private readonly Mock<IUpdatingTweetStore> _fakeStore = new Mock<IUpdatingTweetStore>();
        private readonly TwitterStatus _tweet = New.Tweet;
        private Mock<ITweetScreen> _fakeTweetScreen;

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.GetTweetId(), test.Id);
        }

        [Fact]
        public void Initialize__RequestsTweet()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            InitializeAndWaitUntilStoreIsAskedForTweet(test);

            _fakeStore.Verify(x => x.GetTweet(_tweet.GetTweetId()));
        }

        [Fact]
        public void Initialize_WhenStoreReturnsTweet_OpensTweetScreen()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            _fakeStore.
                Setup(x => x.GetTweet(_tweet.GetTweetId())).
                Returns(_tweet);

            InitializeAndWaitUntilStoreIsAskedForTweet(test);

            _fakeTweetScreen.Verify(x => x.Initialize());
            Assert.Same(_fakeTweetScreen.Object, test.ActiveScreen);
        }

        private LoadingTweetScreen BuildDefaultTestSubject()
        {
            _fakeTweetScreen = new Mock<ITweetScreen>();
            return new LoadingTweetScreen(_fakeStore.Object,
                                          _tweet.GetTweetId(),
                                          x => _fakeTweetScreen.Object);
        }

        private void InitializeAndWaitUntilStoreIsAskedForTweet(LoadingTweetScreen test)
        {
            test.Initialize();
            // TODO: Get rid of Sleep
            Thread.Sleep(1000);
        }
    }
}