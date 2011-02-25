namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using Caliburn.Micro;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.ViewModels;
    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services.Interfaces;

    using Xunit;

    #endregion

    public class ShellViewModelTests
    {
        private readonly IStatusScreen _status = Substitute.For<IStatusScreen>();

        private readonly ITimelineScreen _timeline = Substitute.For<ITimelineScreen>();

        private readonly ITimelineUpdater _updater = Substitute.For<ITimelineUpdater>();

        [Fact]
        public void GettingStatus_WhenInitialized_ReturnsInitializedStatus()
        {
            ShellViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            _status.Received().Activate();
            test.Status.Should().Be.SameAs(_status);
        }

        [Fact]
        public void GettingTimeline_WhenInitialized_ReturnsInitializedTimeline()
        {
            ShellViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            _timeline.Received().Activate();
            test.Timeline.Should().Be.SameAs(_timeline);
        }

        [Fact]
        public void Initialize__StartsRequestingTweets()
        {
            ShellViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            _updater.Received().Start();
        }

        private ShellViewModel BuildDefaultTestSubject()
        {
            return new ShellViewModel(_timeline, _status, _updater);
        }
    }
}