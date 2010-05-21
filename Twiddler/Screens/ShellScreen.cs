using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IShellScreen))]
    public class ShellScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, IShellScreen
    {
        public ShellScreen(ITimelineScreen timelineScreen, IStatusScreen statusScreen) : base(true)
        {
            Timeline = timelineScreen;
            Status = statusScreen;
        }

        public ITimelineScreen Timeline { get; private set; }

        public IStatusScreen Status { get; private set; }
    }
}