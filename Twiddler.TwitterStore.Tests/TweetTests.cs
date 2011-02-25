namespace Twiddler.TwitterStore.Tests
{
    #region Using Directives

    using Should.Fluent;

    using Twiddler.Tests;
    using Twiddler.TwitterStore.Models;

    using Xunit;

    #endregion

    public class TweetTests
    {
        [Fact]
        public void Archive__UpdatesIsArchived()
        {
            var test = new Tweet();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.IsArchived, test.Archive);
        }

        [Fact]
        public void GettingIsRead_Initially_IsFalse()
        {
            var test = new Tweet();
            test.IsRead.Should().Be.False();
        }

        [Fact]
        public void MarkAsRead__UpdatesIsRead()
        {
            var test = new Tweet();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.IsRead, test.MarkAsRead);
        }
    }
}