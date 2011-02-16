namespace Twiddler.Tests.Commands
{
    #region Using Directives

    using NSubstitute;

    using Twiddler.Commands;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class MarkTweetAsReadCommandTests
    {
        private readonly ITweetStore _store = Substitute.For<ITweetStore>();

        private readonly ITweet _tweet = A.Tweet.Build();

        [Fact]
        public void CanExecuteChanged_WhenTweetBecomesRead_IsRaised()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            bool eventRaised = false;
            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            _tweet.MarkAsRead();

            Assert.True(eventRaised);
        }

        [Fact]
        public void CanExecute_WhenTweetIsNotRead_IsTrue()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            Assert.True(test.CanExecute(null));
        }

        [Fact]
        public void CanExecute_WhenTweetIsRead_IsFalse()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            Assert.True(test.CanExecute(null));
        }

        [Fact]
        public void Execute_WhenTweetIsNotRead_MarksTweetAsRead()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            test.Execute(null);

            Assert.True(_tweet.IsRead);
        }

        [Fact]
        public void Execute_WhenTweetIsNotRead_SavesTweetToStore()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            test.Execute(null);

            _store.Received().Add(_tweet);
        }

        private MarkTweetAsReadCommand BuildDefaultTestSubject()
        {
            return new MarkTweetAsReadCommand(_tweet, _store);
        }
    }
}