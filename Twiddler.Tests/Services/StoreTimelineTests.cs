using Twiddler.Services;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class StoreTimelineTests
    {
        [Fact]
        public void GettingTweets_Initially_IsEmpty()
        {
            StoreTimeline test = BuildDefaultTestSubject();

            Assert.Empty(test.Tweets);
        }

        private StoreTimeline BuildDefaultTestSubject()
        {
            return new StoreTimeline();
        }
    }
}