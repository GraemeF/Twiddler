using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetPlaceholderScreen))]
    public class TweetPlaceholderScreen : Screen, ITweetPlaceholderScreen
    {
    }
}