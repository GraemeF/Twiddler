namespace Twiddler.Tests.Services
{
    #region Using Directives

    using NSubstitute;

    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;

    using Xunit;

    #endregion

    public class TimelineUpdaterTests
    {
        private readonly IAsyncTweetFetcher _requestConductor = Substitute.For<IAsyncTweetFetcher>();

        private readonly ITimeline _store = Substitute.For<ITimeline>();

        [Fact]
        public void Start__StartsRequestingTweetsForStore()
        {
            TimelineUpdater test = BuildDefaultTestSubject();
            test.Start();

            _requestConductor.Received().Start(_store);
        }

        private TimelineUpdater BuildDefaultTestSubject()
        {
            return new TimelineUpdater(_requestConductor, _store);
        }
    }
}