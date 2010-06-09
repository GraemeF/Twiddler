using System.Threading;
using Moq;
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
        private readonly Mock<ITweetPlaceholderScreen> _fakeTweetPlaceholderScreen = new Mock<ITweetPlaceholderScreen>();
        private readonly Tweet _tweet = New.Tweet;
        private Mock<ITweetScreen> _fakeTweetScreen;

        public LoadingTweetScreenTests()
        {
            _fakeTweetPlaceholderScreen.
                Setup(x => x.CanShutdown()).
                Returns(true);
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Id, test.Id);
        }

        [Fact]
        public void Initialize__RequestsTweet()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            InitializeAndWaitUntilStoreIsAskedForTweet(test);

            _fakeStore.Verify(x => x.GetTweet(_tweet.Id));
        }

        [Fact]
        public void Initialize__OpensPlaceholderScreen()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            test.Initialize();

            Assert.Same(_fakeTweetPlaceholderScreen.Object, test.ActiveScreen);
        }

        [Fact]
        public void Initialize_WhenStoreReturnsTweet_OpensTweetScreen()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            _fakeStore.
                Setup(x => x.GetTweet(_tweet.Id)).
                Returns(_tweet);

            InitializeAndWaitUntilStoreIsAskedForTweet(test);

            _fakeTweetScreen.Verify(x => x.Initialize());
            Assert.Same(_fakeTweetScreen.Object, test.ActiveScreen);
        }

        private LoadingTweetScreen BuildDefaultTestSubject()
        {
            _fakeTweetScreen = new Mock<ITweetScreen>();
            return new LoadingTweetScreen(_fakeTweetPlaceholderScreen.Object,
                                          _fakeStore.Object,
                                          _tweet.Id,
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