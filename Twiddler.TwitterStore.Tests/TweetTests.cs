namespace Twiddler.TwitterStore.Tests
{
    #region Using Directives

    using Caliburn.Testability.Extensions;

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
                AssertThatChangeNotificationIsRaisedBy(x => x.IsArchived).
                When(test.Archive);
        }

        [Fact]
        public void GettingIsRead_Initially_IsFalse()
        {
            var test = new Tweet();
            Assert.False(test.IsRead);
        }

        [Fact]
        public void MarkAsRead__UpdatesIsRead()
        {
            var test = new Tweet();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.IsRead).
                When(test.MarkAsRead);
        }
    }
}