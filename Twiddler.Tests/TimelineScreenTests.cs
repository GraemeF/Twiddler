using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests
{
    public class TimelineScreenTests
    {
        [Fact]
        public void GettingScreens_WhenThereAreNoTweets_IsEmpty()
        {
            var test = new TimelineScreen();

            Assert.Empty(test.Screens);
        }
    }
}