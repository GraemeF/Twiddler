namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using Caliburn.Core.IoC;

    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(ILinkThumbnailScreenFactory))]
    [Export(typeof(ILinkThumbnailScreenFactory))]
    public class LinkThumbnailScreenFactory : ILinkThumbnailScreenFactory
    {
        private readonly Factories.ImageThumbnailScreenFactory _imageThumbnailScreenFactory;

        [ImportingConstructor]
        public LinkThumbnailScreenFactory(CompositionContainer compositionContainer, 
                                          Factories.ImageThumbnailScreenFactory imageThumbnailScreenFactory)
        {
            _imageThumbnailScreenFactory = imageThumbnailScreenFactory;

            compositionContainer.ComposeParts(this);
        }

        [ImportMany(typeof(IImageUriDecoder))]
        private IEnumerable<IImageUriDecoder> ImageUriDecoders { get; set; }

        #region ILinkThumbnailScreenFactory members

        public ILinkThumbnailScreen CreateScreenForLink(Uri url)
        {
            IImageUriDecoder decoder = ImageUriDecoders.FirstOrDefault(x => x.CanGetImageLocations(url));

            return decoder != null
                       ? _imageThumbnailScreenFactory(decoder.GetImageLocations(url))
                       : null;
        }

        #endregion
    }
}