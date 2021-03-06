﻿namespace Twiddler.Services.ImageServices
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Twiddler.Models;
    using Twiddler.Services.Interfaces;

    #endregion

    [Export(typeof(IImageUriDecoder))]
    public class TwitPicDecoder : IImageUriDecoder
    {
        private static readonly Uri LinkBase = new Uri("http://twitpic.com/");

        private static readonly Uri ThumbBase = new Uri(LinkBase, "show/mini/");

        #region IImageUriDecoder members

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
                           Link = new Uri(LinkBase, imageId), 
                           FullSize = null, 
                           Thumbnail = new Uri(ThumbBase, imageId)
                       };
        }

        #endregion
    }
}