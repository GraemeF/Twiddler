namespace Twiddler.Services
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(ITimelineUpdater))]
    [Export(typeof(ITimelineUpdater))]
    public class TimelineUpdater : ITimelineUpdater
    {
        private readonly IRequestConductor _requestConductor;

        private readonly ITimeline _timeline;

        [ImportingConstructor]
        public TimelineUpdater(IRequestConductor requestConductor, ITimeline timeline)
        {
            _requestConductor = requestConductor;
            _timeline = timeline;
        }

        #region ITimelineUpdater members

        public void Start()
        {
            _requestConductor.Start(_timeline);
        }

        #endregion
    }
}