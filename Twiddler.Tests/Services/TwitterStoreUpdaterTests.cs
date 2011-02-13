namespace Twiddler.Tests.Services
{
    #region Using Directives

    using Moq;

    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;

    using Xunit;

    #endregion

    public class TimelineUpdaterTests
    {
        private readonly Mock<IAsyncTweetFetcher> _fakeRequestConductor = new Mock<IAsyncTweetFetcher>();

        private readonly Mock<ITimeline> _fakeStore = new Mock<ITimeline>();

        [Fact]
        public void Start__StartsRequestingTweetsForStore()
        {
            TimelineUpdater test = BuildDefaultTestSubject();
            test.Start();

            _fakeRequestConductor.Verify(x => x.Start(_fakeStore.Object));
        }

        private TimelineUpdater BuildDefaultTestSubject()
        {
            return new TimelineUpdater(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}