using System;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services.ImageServices
{
    [PerRequest("yfrog.com", typeof (IImageUriDecoder))]
    public class YFrogDecoder : IImageUriDecoder
    {
        private readonly string[] _hosts = {
                                               "yfrog.com",
                                               "yfrog.ru",
                                               "yfrog.com.tr",
                                               "yfrog.it",
                                               "yfrog.fr",
                                               "yfrog.co.il",
                                               "yfrog.co.uk",
                                               "yfrog.com.pl",
                                               "yfrog.pl",
                                               "yfrog.eu"
                                           };

        #region IImageUriDecoder Members

        public bool CanGetImageLocations(Uri uri)
        {
            string id = uri.PathAndQuery;

            return _hosts.Any(host => uri.Host.Equals(host, StringComparison.InvariantCultureIgnoreCase)) &&
                   id.Length > 3 &&
                   IsContentTypeWithThumbnail(id);
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

        private static bool IsContentTypeWithThumbnail(string id)
        {
            return !(id.EndsWith("s") || id.EndsWith("d"));
        }
    }
}