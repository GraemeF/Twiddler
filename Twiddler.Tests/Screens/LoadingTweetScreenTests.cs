using Moq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class LoadingTweetScreenTests
    {
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();
        private readonly Mock<ITweetPlaceholderScreen> _fakeTweetPlaceholderScreen = new Mock<ITweetPlaceholderScreen>();
        private readonly Tweet _tweet = New.Tweet;
        private Mock<ITweetScreen> _fakeTweetScreen;
        private bool _storeAskedForTweet;

        public LoadingTweetScreenTests()
        {
            _fakeTweetPlaceholderScreen.
                Setup(x => x.CanShutdown()).
                Returns(true);

            _fakeStore.
                Setup(x => x.GetTweet(_tweet.Id)).
                Callback(() => _storeAskedForTweet = true).
                Returns(_tweet);
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
            Wait.Until(() => _storeAskedForTweet);
        }
    }
}