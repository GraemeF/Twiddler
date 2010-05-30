using System;
using Moq;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class LinkThumbnailScreenFactoryTests
    {
        private readonly Mock<IImageUriDecoder> _fakeDecoder = new Mock<IImageUriDecoder>();
        private readonly Uri _knownUri = new Uri("http://known.host.com/a/picture");
        private readonly Uri _unknownUri = new Uri("http://unknown.host.com/a/picture");
        private IImageThumbnailScreen _imageThumbnailScreen = new Mock<IImageThumbnailScreen>().Object;

        public LinkThumbnailScreenFactoryTests()
        {
            _fakeDecoder.
                Setup(x => x.CanGetImageLocations(_knownUri)).
                Returns(true);

            _fakeDecoder.
                Setup(x => x.GetImageLocations(_knownUri)).
                Returns(new ImageLocations());
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithRegisteredHost_ReturnsInstanceForHost()
        {
            var test = new LinkThumbnailScreenFactory(new[] {_fakeDecoder.Object},
                                                      x => _imageThumbnailScreen);

            test.CreateScreenForLink(_knownUri);
        }

        [Fact]
        public void CreateScreenForLink_GivenUrlWithUnregisteredHost_ReturnsNull()
        {
            var test = new LinkThumbnailScreenFactory(new[] {_fakeDecoder.Object},
                                                      x => _imageThumbnailScreen);

            test.CreateScreenForLink(_unknownUri);
        }
    }
}