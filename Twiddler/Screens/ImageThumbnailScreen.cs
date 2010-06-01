using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Commands.Interfaces;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IImageThumbnailScreen))]
    public class ImageThumbnailScreen : Screen<Uri>, IImageThumbnailScreen
    {
        public ImageThumbnailScreen(ImageLocations imageLocations, IOpenLinkCommand openLinkCommand)
        {
            OpenLinkCommand = openLinkCommand;
            Link = imageLocations.Link;
            Thumbnail = imageLocations.Thumbnail;
            FullSize = imageLocations.FullSize;
        }

        public IOpenLinkCommand OpenLinkCommand { get; private set; }

        public Uri Link { get; private set; }
        public Uri Thumbnail { get; private set; }
        public Uri FullSize { get; private set; }
    }
}