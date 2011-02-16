namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    using NSubstitute;

    using Should.Fluent;

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

        private readonly IImageThumbnailScreen _imageThumbnailScreen = Substitute.For<IImageThumbnailScreen>();

        public LinkThumbnailScreenFactoryTests()
        {
            _compostionContainer = new CompositionContainer(new TypeCatalog(typeof(TestDecoder)));
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithRegisteredHost_ReturnsInstanceForHost()
        {
            LinkThumbnailScreenFactory test = BuildDefaultTestSubject();

            test.CreateScreenForLink(KnownUri).Should().Be.SameAs(_imageThumbnailScreen);
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithUnregisteredHost_ReturnsNull()
        {
            LinkThumbnailScreenFactory test = BuildDefaultTestSubject();

            test.CreateScreenForLink(UnknownUri).Should().Be.Null();
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