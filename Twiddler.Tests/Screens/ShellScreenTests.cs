using Moq;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class ShellScreenTests
    {
        private readonly Mock<IStatusScreen> _fakeStatus = new Mock<IStatusScreen>();
        private readonly Mock<ITimelineScreen> _fakeTimeline = new Mock<ITimelineScreen>();
        private readonly Mock<ITimelineUpdater> _fakeUpdater = new Mock<ITimelineUpdater>();

        [Fact]
        public void GettingTimeline_WhenInitialized_ReturnsInitializedTimeline()
        {
            ShellScreen test = BuildDefaultTestSubject();
            test.Initialize();

            _fakeTimeline.Verify(x => x.Initialize());
            Assert.Same(_fakeTimeline.Object, test.Timeline);
        }

        [Fact]
        public void GettingStatus_WhenInitialized_ReturnsInitializedStatus()
        {
            ShellScreen test = BuildDefaultTestSubject();
            test.Initialize();

            _fakeStatus.Verify(x => x.Initialize());
            Assert.Same(_fakeStatus.Object, test.Status);
        }

        [Fact]
        public void Initialize__StartsRequestingTweets()
        {
            ShellScreen test = BuildDefaultTestSubject();
            test.Initialize();

            _fakeUpdater.Verify(x => x.Start());
        }

        private ShellScreen BuildDefaultTestSubject()
        {
            return new ShellScreen(_fakeTimeline.Object, _fakeStatus.Object, _fakeUpdater.Object);
        }
    }
}