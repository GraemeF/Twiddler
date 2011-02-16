namespace Twiddler.Tests.Commands
{
    #region Using Directives

    using Should.Fluent;

    using Twiddler.Commands;
    using Twiddler.Core.Models;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class ArchiveTweetCommandTests
    {
        [Fact]
        public void CanExecuteChanged_WhenTweetBecomesArchived_IsRaised()
        {
            ITweet tweet = A.Tweet.Build();
            var test = new ArchiveTweetCommand(tweet);

            bool eventRaised = false;
            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            tweet.Archive();

            eventRaised.Should().Be.True();
        }

        [Fact]
        public void CanExecute_WhenTweetIsArchived_IsFalse()
        {
            var test = new ArchiveTweetCommand(A.Tweet.Build());

            test.CanExecute(null).Should().Be.True();
        }

        [Fact]
        public void CanExecute_WhenTweetIsNotArchived_IsTrue()
        {
            var test = new ArchiveTweetCommand(A.Tweet.Build());

            test.CanExecute(null).Should().Be.True();
        }

        [Fact]
        public void Execute_WhenTweetIsNotArchived_ArchivesTweet()
        {
            ITweet tweet = A.Tweet.Build();
            var test = new ArchiveTweetCommand(tweet);

            test.Execute(null);

            tweet.IsArchived.Should().Be.True();
        }
    }
}