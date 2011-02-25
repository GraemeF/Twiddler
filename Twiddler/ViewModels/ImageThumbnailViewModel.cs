namespace Twiddler.ViewModels
{
    #region Using Directives

    using System;

    using Caliburn.Micro;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Models;
    using Twiddler.ViewModels.Interfaces;

    #endregion

    public class ImageThumbnailViewModel : Screen, 
                                           IImageThumbnailScreen
    {
        public ImageThumbnailViewModel(ImageLocations imageLocations, IOpenLinkCommand openLinkCommand)
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