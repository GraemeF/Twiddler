using System.ComponentModel.Composition;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [Export(typeof (IShellScreen))]
    public class ShellScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, IShellScreen
    {
        private readonly ITwitterStoreUpdater _twitterStoreUpdater;

        [ImportingConstructor]
        public ShellScreen(ITimelineScreen timelineScreen,
                           IStatusScreen statusScreen,
                           ITwitterStoreUpdater twitterStoreUpdater)
            : base(true)
        {
            _twitterStoreUpdater = twitterStoreUpdater;
            Timeline = timelineScreen;
            Status = statusScreen;
        }

        public ITimelineScreen Timeline { get; private set; }

        public IStatusScreen Status { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _twitterStoreUpdater.Start();
        }
    }
}