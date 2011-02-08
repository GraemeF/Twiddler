namespace Twiddler.Screens
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;
    using Caliburn.PresentationFramework.Screens;

    using Twiddler.Screens.Interfaces;

    #endregion

    [PerRequest(typeof(ITweetPlaceholderScreen))]
    [Export(typeof(ITweetPlaceholderScreen))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TweetPlaceholderScreen : Screen, 
                                          ITweetPlaceholderScreen
    {
    }
}