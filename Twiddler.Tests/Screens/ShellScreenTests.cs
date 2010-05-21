using Moq;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class ShellScreenTests
    {
        private readonly Mock<IStatusScreen> _fakeStatus = new Mock<IStatusScreen>();
        private readonly Mock<ITimelineScreen> _fakeTimeline = new Mock<ITimelineScreen>();

        [Fact]
        public void GettingTimeline_WhenInitialized_ReturnsInitializedTimeline()
        {
            var test = new ShellScreen(_fakeTimeline.Object, _fakeStatus.Object);
            test.Initialize();

            _fakeTimeline.Verify(x => x.Initialize());
            Assert.Same(_fakeTimeline.Object, test.Timeline);
        }

        [Fact]
        public void GettingStatus_WhenInitialized_ReturnsInitializedStatus()
        {
            var test = new ShellScreen(_fakeTimeline.Object, _fakeStatus.Object);
            test.Initialize();

            _fakeStatus.Verify(x => x.Initialize());
            Assert.Same(_fakeStatus.Object, test.Status);
        }
    }
}