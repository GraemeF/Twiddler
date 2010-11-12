using System;
using Twiddler.Models;
using Twiddler.Services.ImageServices;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Services.ImageServices
{
    public class TwitPicDecoderTests
    {
        [Theory]
        [InlineData("http://twitpic.com/1e10q")]
        [InlineData("http://twitpic.com/show/thumb/1e10q")]
        public void CanGetImageLocations_GivenTwitPicImageUrl_ReturnsTrue(string url)
        {
            Assert.True(new TwitPicDecoder().CanGetImageLocations(new Uri(url)));
        }

        [Theory]
        [InlineData("http://some.other.com/")]
        [InlineData("http://twitpic.com/")]
        public void CanGetImageLocations_GivenOtherUrl_ReturnsFalse(string url)
        {
            Assert.False(new TwitPicDecoder().CanGetImageLocations(new Uri(url)));
        }

        [Theory]
        [InlineData("http://twitpic.com/1e10q", "http://twitpic.com/show/mini/1e10q")]
        [InlineData("http://twitpic.com/show/thumb/1e10q", "http://twitpic.com/show/mini/1e10q")]
        [InlineData("http://twitpic.com/show/mini/1e10q", "http://twitpic.com/show/mini/1e10q")]
        public void GetImageLocations_GivenTwitPicImageUrl_ReturnsThumbnailLocation(string url, string linkUrl)
        {
            ImageLocations locations = new TwitPicDecoder().GetImageLocations(new Uri(url));

            Assert.Equal(linkUrl, locations.Thumbnail.ToString());
        }

        [Theory]
        [InlineData("http://twitpic.com/1e10q", "http://twitpic.com/1e10q")]
        [InlineData("http://twitpic.com/show/thumb/1e10q", "http://twitpic.com/1e10q")]
        [InlineData("http://twitpic.com/show/mini/1e10q", "http://twitpic.com/1e10q")]
        public void GetImageLocations_GivenTwitPicImageUrl_ReturnsLinkLocation(string url, string linkUrl)
        {
            ImageLocations locations = new TwitPicDecoder().GetImageLocations(new Uri(url));

            Assert.Equal(linkUrl, locations.Link.ToString());
        }

        [Fact]
        public void GetImageLocations_GivenTwitPicImageUrl_ReturnsLinkLocation()
        {
            ImageLocations locations = new TwitPicDecoder().GetImageLocations(new Uri("http://twitpic.com/1e10q"));

            Assert.Equal("http://twitpic.com/1e10q", locations.Link.ToString());
        }
    }
}