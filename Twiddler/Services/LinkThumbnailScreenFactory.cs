using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ILinkThumbnailScreenFactory))]
    public class LinkThumbnailScreenFactory : ILinkThumbnailScreenFactory
    {
        private readonly Factories.ImageThumbnailScreenFactory _imageThumbnailScreenFactory;
        private readonly IEnumerable<IImageUriDecoder> _imageUriDecoders;

        public LinkThumbnailScreenFactory(IEnumerable<IImageUriDecoder> imageUriDecoders,
                                          Factories.ImageThumbnailScreenFactory imageThumbnailScreenFactory)
        {
            _imageUriDecoders = imageUriDecoders;
            _imageThumbnailScreenFactory = imageThumbnailScreenFactory;
        }

        #region ILinkThumbnailScreenFactory Members

        public ILinkThumbnailScreen CreateScreenForLink(Uri url)
        {
            IImageUriDecoder decoder = _imageUriDecoders.FirstOrDefault(x => x.CanGetImageLocations(url));

            return decoder != null
                       ? _imageThumbnailScreenFactory(decoder.GetImageLocations(url))
                       : null;
        }

        #endregion
    }
}