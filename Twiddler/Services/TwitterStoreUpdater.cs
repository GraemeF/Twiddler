namespace Twiddler.Services
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using Twiddler.Core.Services;
    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(ITimelineUpdater))]
    [Export(typeof(ITimelineUpdater))]
    public class TimelineUpdater : ITimelineUpdater
    {
        private readonly IAsyncTweetFetcher _asyncTweetFetcher;

        private readonly ITimeline _timeline;

        [ImportingConstructor]
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