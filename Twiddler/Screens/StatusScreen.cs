using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IStatusScreen))]
    public class StatusScreen : Screen, IStatusScreen
    {
    }
}