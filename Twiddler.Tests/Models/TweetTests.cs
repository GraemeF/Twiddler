using Caliburn.Testability.Extensions;
using Twiddler.Core.Models;
using Twiddler.Models;
using Xunit;

namespace Twiddler.Tests.Models
{
    public class TweetTests
    {
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