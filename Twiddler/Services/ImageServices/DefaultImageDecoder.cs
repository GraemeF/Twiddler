namespace Twiddler.Services.ImageServices
{
    #region Using Directives

    using System;

    using Twiddler.Models;
    using Twiddler.Services.Interfaces;

    #endregion

    public class DefaultImageDecoder : IDefaultImageUriDecoder
    {
        #region IImageUriDecoder members

        public bool CanGetImageLocations(Uri uri)
        {
            return true;
        }

        public ImageLocations GetImageLocations(Uri uri)
        {
            return new ImageLocations
                       {
                           FullSize = null, 
                           Link = uri, 
                           Thumbnail = new Uri(uri.GetLeftPart(UriPartial.Authority) + "/favicon.ico")
                       };
        }

        #endregion
    }
}