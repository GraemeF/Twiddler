using Twiddler.Screens.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TweetScreenTests
    {
        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            var test = new TweetScreen(New.Tweet);

            Assert.Equal("Unspecified", test.Status);
        }
    }
}