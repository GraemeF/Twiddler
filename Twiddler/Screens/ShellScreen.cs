using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IShellScreen))]
    public class ShellScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, IShellScreen
    {
        public ShellScreen(ITimelineScreen timelineScreen) : base(true)
        {
            Timeline = timelineScreen;
        }

        public ITimelineScreen Timeline { get; private set; }
    }
}