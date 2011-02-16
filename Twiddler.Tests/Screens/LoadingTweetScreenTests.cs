namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Screens;
    using Twiddler.Screens.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class LoadingTweetScreenTests
    {
        private readonly ITweetStore _store = Substitute.For<ITweetStore>();

        private readonly ITweet _tweet = A.Tweet.Build();

        private readonly ITweetPlaceholderScreen _tweetPlaceholderScreen = Substitute.For<ITweetPlaceholderScreen>();

        private bool _storeAskedForTweet;

        private ITweetScreen _tweetScreen;

        public LoadingTweetScreenTests()
        {
            _tweetPlaceholderScreen.CanShutdown().Returns(true);

            _store.GetTweet(_tweet.Id).Returns(_ =>
                {
                    _storeAskedForTweet = true;
                    return _tweet;
                });
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            test.Id.Should().Equal(_tweet.Id);
        }

        [Fact]
        public void Initialize_WhenStoreReturnsTweet_OpensTweetScreen()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            InitializeAndWaitUntilStoreIsAskedForTweet(test);

            _tweetScreen.Received().Initialize();
            test.ActiveScreen.Should().Be.SameAs(_tweetScreen);
        }

        [Fact]
        public void Initialize__OpensPlaceholderScreen()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            test.Initialize();

            test.ActiveScreen.Should().Be.SameAs(_tweetPlaceholderScreen);
        }

        [Fact]
        public void Initialize__RequestsTweet()
        {
            LoadingTweetScreen test = BuildDefaultTestSubject();

            InitializeAndWaitUntilStoreIsAskedForTweet(test);

            _store.Received().GetTweet(_tweet.Id);
        }

        private LoadingTweetScreen BuildDefaultTestSubject()
        {
            _tweetScreen = Substitute.For<ITweetScreen>();
            return new LoadingTweetScreen(_tweetPlaceholderScreen, 
                                          _store, 
                                          _tweet.Id, 
                                          x => _tweetScreen);
        }

        private void InitializeAndWaitUntilStoreIsAskedForTweet(LoadingTweetScreen test)
        {
            test.Initialize();
            Wait.Until(() => _storeAskedForTweet);
        }
    }
}