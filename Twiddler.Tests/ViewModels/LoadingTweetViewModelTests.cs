namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using Caliburn.Micro;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TestData;
    using Twiddler.ViewModels;
    using Twiddler.ViewModels.Interfaces;

    using Xunit;

    #endregion

    public class LoadingTweetViewModelTests
    {
        private readonly ITweetStore _store = Substitute.For<ITweetStore>();

        private readonly ITweet _tweet = A.Tweet.Build();

        private readonly ITweetPlaceholderScreen _tweetPlaceholderScreen = Substitute.For<ITweetPlaceholderScreen>();

        private bool _storeAskedForTweet;

        private ITweetScreen _tweetScreen;

        public LoadingTweetViewModelTests()
        {
            _store.GetTweet(_tweet.Id).Returns(_ =>
                {
                    _storeAskedForTweet = true;
                    return _tweet;
                });
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            LoadingTweetViewModel test = BuildDefaultTestSubject();

            test.Id.Should().Equal(_tweet.Id);
        }

        [Fact]
        public void Initialize_WhenStoreReturnsTweet_OpensTweetScreen()
        {
            LoadingTweetViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            WaitUntilStoreIsAskedForTweet();

            _tweetScreen.Received().Activate();
            test.ActiveItem.Should().Be.SameAs(_tweetScreen);
        }

        [Fact]
        public void Initialize__OpensPlaceholderScreen()
        {
            _store.GetTweet(Arg.Any<string>()).Returns(_ => null);

            LoadingTweetViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            _tweetPlaceholderScreen.Received().Activate();
            test.ActiveItem.Should().Be.SameAs(_tweetPlaceholderScreen);
        }

        [Fact]
        public void Initialize__RequestsTweet()
        {
            LoadingTweetViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            WaitUntilStoreIsAskedForTweet();

            _store.Received().GetTweet(_tweet.Id);
        }

        private LoadingTweetViewModel BuildDefaultTestSubject()
        {
            _tweetScreen = Substitute.For<ITweetScreen>();
            return new LoadingTweetViewModel(_tweetPlaceholderScreen, 
                                             _store, 
                                             _tweet.Id, 
                                             x => _tweetScreen);
        }

        private void WaitUntilStoreIsAskedForTweet()
        {
            Wait.Until(() => _storeAskedForTweet);
        }
    }
}