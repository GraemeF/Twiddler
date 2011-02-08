namespace Twiddler.Screens
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;
    using Caliburn.PresentationFramework.Screens;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Models;
    using Twiddler.Screens.Interfaces;

    #endregion

    [PerRequest(typeof(IImageThumbnailScreen))]
    [Export(typeof(IImageThumbnailScreen))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ImageThumbnailScreen : Screen<Uri>, 
                                        IImageThumbnailScreen
    {
        [ImportingConstructor]
        public ImageThumbnailScreen(ImageLocations imageLocations, IOpenLinkCommand openLinkCommand)
        {
            OpenLinkCommand = openLinkCommand;
            Link = imageLocations.Link;
            Thumbnail = imageLocations.Thumbnail;
            FullSize = imageLocations.FullSize;
        }

        public Uri FullSize { get; private set; }

        public Uri Link { get; private set; }

        public IOpenLinkCommand OpenLinkCommand { get; private set; }

        public Uri Thumbnail { get; private set; }
    }
}