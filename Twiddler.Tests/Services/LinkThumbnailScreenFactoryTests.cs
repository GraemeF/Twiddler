namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Models;
    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;

    using Xunit;

    #endregion

    public class LinkThumbnailScreenFactoryTests
    {
        private static readonly Uri KnownUri = new Uri("http://known.host.com/a/picture");

        private static readonly Uri UnknownUri = new Uri("http://unknown.host.com/a/picture");

        private readonly CompositionContainer _compostionContainer;

        private readonly ImageLocations _defaultImageLocations = new ImageLocations();

        private readonly IImageThumbnailScreen _defaultImageThumbnailScreen = Substitute.For<IImageThumbnailScreen>();

        private readonly IDefaultImageUriDecoder _defaultImageUriDecoder = Substitute.For<IDefaultImageUriDecoder>();

        private readonly IImageThumbnailScreen _imageThumbnailScreen = Substitute.For<IImageThumbnailScreen>();

        public LinkThumbnailScreenFactoryTests()
        {
            _compostionContainer = new CompositionContainer(new TypeCatalog(typeof(TestDecoder)));
            _defaultImageUriDecoder.GetImageLocations(Arg.Any<Uri>()).Returns(_defaultImageLocations);
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithRegisteredHost_ReturnsInstanceForHost()
        {
            LinkThumbnailScreenFactory test = BuildDefaultTestSubject();

            test.CreateScreenForLink(KnownUri).Should().Be.SameAs(_imageThumbnailScreen);
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithUnregisteredHost_ReturnsDefault()
        {
            LinkThumbnailScreenFactory test = BuildDefaultTestSubject();

            test.CreateScreenForLink(UnknownUri).Should().Be.SameAs(_defaultImageThumbnailScreen);
        }

        private LinkThumbnailScreenFactory BuildDefaultTestSubject()
        {
            return new LinkThumbnailScreenFactory(_compostionContainer, 
                                                  _defaultImageUriDecoder, 
                                                  x => x == _defaultImageLocations
                                                           ? _defaultImageThumbnailScreen
                                                           : _imageThumbnailScreen);
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