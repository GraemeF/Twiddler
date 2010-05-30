using System;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("twitpic.com", typeof (IImageUriDecoder))]
    public class TwitpicDecoder : IImageUriDecoder
    {
        #region IImageUriDecoder Members

        public bool CanGetImageLocations(Uri uri)
        {
            return uri.Host.Equals("twitpic.com", StringComparison.InvariantCultureIgnoreCase);
        }

        public ImageLocations GetImageLocations(Uri uri)
        {
            return new ImageLocations
                       {
                           Link = uri,
                           FullSize = null,
                           Thumbnail = new Uri("http://twitpic.com/show/thumb" + uri.PathAndQuery)
                       };
        }

        #endregion
    }
}