namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    using Moq;

    using Twiddler.Models;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;

    using Xunit;

    #endregion

    public class LinkThumbnailScreenFactoryTests
    {
        private static readonly Uri KnownUri = new Uri("http://known.host.com/a/picture");

        private static readonly Uri UnknownUri = new Uri("http://unknown.host.com/a/picture");

        private readonly CompositionContainer _compostionContainer;

        private readonly IImageThumbnailScreen _imageThumbnailScreen = new Mock<IImageThumbnailScreen>().Object;

        public LinkThumbnailScreenFactoryTests()
        {
            _compostionContainer = new CompositionContainer(new TypeCatalog(typeof(TestDecoder)));
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithRegisteredHost_ReturnsInstanceForHost()
        {
            LinkThumbnailScreenFactory test = BuildDefaultTestSubject();

            Assert.Same(_imageThumbnailScreen, test.CreateScreenForLink(KnownUri));
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithUnregisteredHost_ReturnsNull()
        {
            LinkThumbnailScreenFactory test = BuildDefaultTestSubject();

            Assert.Null(test.CreateScreenForLink(UnknownUri));
        }

        private LinkThumbnailScreenFactory BuildDefaultTestSubject()
        {
            return new LinkThumbnailScreenFactory(_compostionContainer, 
                                                  x => _imageThumbnailScreen);
        }

        [Export(typeof(IImageUriDecoder))]
        public class TestDecoder : IImageUriDecoder
        {
            #region IImageUriDecoder members

            public bool CanGetImageLocations(Uri uri)
            {
                return uri == KnownUri;
            }

            public ImageLocations GetImageLocations(Uri uri)
            {
                return new ImageLocations();
            }

            #endregion
        }
    }
}