using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetPlaceholderScreen))]
    [Export(typeof (ITweetPlaceholderScreen))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TweetPlaceholderScreen : Screen, ITweetPlaceholderScreen
    {
    }
}