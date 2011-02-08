namespace Twiddler.Screens
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;
    using Caliburn.PresentationFramework.Screens;

    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(IShellScreen))]
    [Export(typeof(IShellScreen))]
    public class ShellScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, 
                               IShellScreen
    {
        private readonly ITimelineUpdater _timelineUpdater;

        [ImportingConstructor]
        public ShellScreen(ITimelineScreen timelineScreen, 
                           IStatusScreen statusScreen, 
                           ITimelineUpdater timelineUpdater)
            : base(true)
        {
            _timelineUpdater = timelineUpdater;
            Timeline = timelineScreen;
            Status = statusScreen;
        }

        public IStatusScreen Status { get; private set; }

        public ITimelineScreen Timeline { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _timelineUpdater.Start();
        }
    }
}