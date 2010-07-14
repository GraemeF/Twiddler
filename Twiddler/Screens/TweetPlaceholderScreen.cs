using System.ComponentModel.Composition;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [Export(typeof (ITweetPlaceholderScreen))]
    public class TweetPlaceholderScreen : Screen, ITweetPlaceholderScreen
    {
    }
}