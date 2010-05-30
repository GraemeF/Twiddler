using System;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("yfrog.com", typeof (IImageUriDecoder))]
    public class YfrogDecoder : IImageUriDecoder
    {
        #region IImageUriDecoder Members

        public bool CanGetImageLocations(Uri uri)
        {
            return uri.Host.Equals("yfrog.com",StringComparison.InvariantCultureIgnoreCase);
        }

        public ImageLocations GetImageLocations(Uri uri)
        {
            return new ImageLocations
                       {
                           FullSize = null,
                           Link = uri,
                           Thumbnail = new Uri(uri + ".th.jpg")
                       };
        }

        #endregion
    }
}