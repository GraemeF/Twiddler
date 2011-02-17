namespace Twiddler.Services
{
    #region Using Directives

    using Twiddler.Core.Services;
    using Twiddler.Services.Interfaces;

    #endregion

    public class TimelineUpdater : ITimelineUpdater
    {
        private readonly IAsyncTweetFetcher _asyncTweetFetcher;

        private readonly ITimeline _timeline;

        public TimelineUpdater(IAsyncTweetFetcher asyncTweetFetcher, ITimeline timeline)
        {
            _asyncTweetFetcher = asyncTweetFetcher;
            _timeline = timeline;
        }

        #region ITimelineUpdater members

        public void Start()
        {
            _asyncTweetFetcher.Start(_timeline);
        }

        #endregion
    }
}