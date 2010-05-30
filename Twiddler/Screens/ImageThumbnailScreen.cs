using System;
using System.Diagnostics;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IImageThumbnailScreen))]
    public class ImageThumbnailScreen : Screen<Uri>, IImageThumbnailScreen
    {
        public ImageThumbnailScreen(ImageLocations imageLocations)
        {
            Link = imageLocations.Link;
            Thumbnail = imageLocations.Thumbnail;
            FullSize = imageLocations.FullSize;
        }

        public Uri Link { get; set; }
        public Uri Thumbnail { get; set; }
        public Uri FullSize { get; set; }

        public void OpenLink()
        {
            Process.Start(Link.ToString(), "");
        }
    }
}