using Moq;
using Twiddler.Core.Services;
using Twiddler.Services;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class StoreTimelineTests
    {
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void GettingTweets_Initially_IsEmpty()
        {
            StoreTimeline test = BuildDefaultTestSubject();

            Assert.Empty(test.Tweets);
        }

        [Fact]
        public void GettingTweets_WhenTheStoreHasInboxTweets_ContainsTweets()
        {
            var test = BuildDefaultTestSubject();

        }

        private StoreTimeline BuildDefaultTestSubject()
        {
            return new StoreTimeline(_fakeStore.Object);
        }
    }
}