using Twiddler.Commands;
using Twiddler.Core.Models;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Commands
{
    public class MarkTweetAsReadCommandTests
    {
        private readonly Tweet tweet = New.Tweet;

        [Fact]
        public void Execute_WhenTweetIsNotRead_MarksTweetAsRead()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            test.Execute(null);

            Assert.True(tweet.IsRead);
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
        public void CanExecuteChanged_WhenTweetBecomesRead_IsRaised()
        {
            MarkTweetAsReadCommand test = BuildDefaultTestSubject();

            bool eventRaised = false;
            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            tweet.MarkAsRead();

            Assert.True(eventRaised);
        }

        private MarkTweetAsReadCommand BuildDefaultTestSubject()
        {
            return new MarkTweetAsReadCommand(tweet);
        }
    }
}