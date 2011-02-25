namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    public class LinkThumbnailScreenFactory : ILinkThumbnailScreenFactory
    {
        private readonly IDefaultImageUriDecoder _defaultImageUriDecoder;

        private readonly Factories.ImageThumbnailScreenFactory _imageThumbnailScreenFactory;

        public LinkThumbnailScreenFactory(CompositionContainer compositionContainer, 
                                          IDefaultImageUriDecoder defaultImageUriDecoder, 
                                          Factories.ImageThumbnailScreenFactory imageThumbnailScreenFactory)
        {
            _defaultImageUriDecoder = defaultImageUriDecoder;
            _imageThumbnailScreenFactory = imageThumbnailScreenFactory;

            compositionContainer.ComposeParts(this);
        }

        [ImportMany(typeof(IImageUriDecoder))]
        private IEnumerable<IImageUriDecoder> ImageUriDecoders { get; set; }

        #region ILinkThumbnailScreenFactory members

        public ILinkThumbnailScreen CreateScreenForLink(Uri url)
        {
            IImageUriDecoder decoder = ImageUriDecoders.FirstOrDefault(x => x.CanGetImageLocations(url)) ??
                                       _defaultImageUriDecoder;

            return decoder != null
                       ? _imageThumbnailScreenFactory(decoder.GetImageLocations(url))
                       : null;
        }

        #endregion
    }
}