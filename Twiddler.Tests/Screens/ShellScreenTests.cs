using Moq;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class ShellScreenTests
    {
        [Fact]
        public void GettingTimeline_WhenInitialized_ReturnsInitializedTimeline()
        {
            var mockTimeline = new Mock<ITimelineScreen>();

            var test = new ShellScreen(mockTimeline.Object);
            test.Initialize();

            Assert.Same(mockTimeline.Object, test.Timeline);
        }
    }
}