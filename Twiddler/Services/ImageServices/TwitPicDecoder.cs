using System;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services.ImageServices
{
    [PerRequest("twitpic.com", typeof (IImageUriDecoder))]
    public class TwitPicDecoder : IImageUriDecoder
    {
        private static readonly Uri linkBase = new Uri("http://twitpic.com/");
        private static readonly Uri thumbBase = new Uri(linkBase, "show/mini/");

        #region IImageUriDecoder Members

        public bool CanGetImageLocations(Uri uri)
        {
            return uri.Host.Equals("twitpic.com", StringComparison.InvariantCultureIgnoreCase) &&
                   uri.PathAndQuery.Length > 3;
        }

        public ImageLocations GetImageLocations(Uri uri)
        {
            string imageId = uri.PathAndQuery.Split('/').Last();

            return new ImageLocations
                       {
                           Link = new Uri(linkBase, imageId),
                           FullSize = null,
                           Thumbnail = new Uri(thumbBase, imageId)
                       };
        }

        #endregion
    }
}