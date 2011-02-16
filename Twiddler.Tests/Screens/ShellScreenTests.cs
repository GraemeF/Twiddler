namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using NSubstitute;

    using Twiddler.Screens;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;

    using Xunit;

    #endregion

    public class ShellScreenTests
    {
        private readonly IStatusScreen _status = Substitute.For<IStatusScreen>();

        private readonly ITimelineScreen _timeline = Substitute.For<ITimelineScreen>();

        private readonly ITimelineUpdater _updater = Substitute.For<ITimelineUpdater>();

        [Fact]
        public void GettingStatus_WhenInitialized_ReturnsInitializedStatus()
        {
            ShellScreen test = BuildDefaultTestSubject();
            test.Initialize();

            _status.Received().Initialize();
            Assert.Same(_status, test.Status);
        }

        [Fact]
        public void GettingTimeline_WhenInitialized_ReturnsInitializedTimeline()
        {
            ShellScreen test = BuildDefaultTestSubject();
            test.Initialize();

            _timeline.Received().Initialize();
            Assert.Same(_timeline, test.Timeline);
        }

        [Fact]
        public void Initialize__StartsRequestingTweets()
        {
            ShellScreen test = BuildDefaultTestSubject();
            test.Initialize();

            _updater.Received().Start();
        }

        private ShellScreen BuildDefaultTestSubject()
        {
            return new ShellScreen(_timeline, _status, _updater);
        }
    }
}