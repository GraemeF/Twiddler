namespace Twiddler.Tests.Services.ImageServices
{
    #region Using Directives

    using System;

    using Should.Fluent;

    using Twiddler.Models;
    using Twiddler.Services.ImageServices;

    using Xunit.Extensions;

    #endregion

    public class TwitPicDecoderTests
    {
        [Theory]
        [InlineData("http://some.other.com/")]
        [InlineData("http://twitpic.com/")]
        public void CanGetImageLocations_GivenOtherUrl_ReturnsFalse(string url)
        {
            new TwitPicDecoder().CanGetImageLocations(new Uri(url)).Should().Be.False();
        }

        [Theory]
        [InlineData("http://twitpic.com/1e10q")]
        [InlineData("http://twitpic.com/show/thumb/1e10q")]
        public void CanGetImageLocations_GivenTwitPicImageUrl_ReturnsTrue(string url)
        {
            new TwitPicDecoder().CanGetImageLocations(new Uri(url)).Should().Be.True();
        }

        [Theory]
        [InlineData("http://twitpic.com/1e10q", "http://twitpic.com/1e10q")]
        [InlineData("http://twitpic.com/show/thumb/1e10q", "http://twitpic.com/1e10q")]
        [InlineData("http://twitpic.com/show/mini/1e10q", "http://twitpic.com/1e10q")]
        public void GetImageLocations_GivenTwitPicImageUrl_ReturnsLinkLocation(string url, string linkUrl)
        {
            ImageLocations locations = new TwitPicDecoder().GetImageLocations(new Uri(url));

            locations.Link.ToString().Should().Equal(linkUrl);
        }

        [Theory]
        [InlineData("http://twitpic.com/1e10q", "http://twitpic.com/show/mini/1e10q")]
        [InlineData("http://twitpic.com/show/thumb/1e10q", "http://twitpic.com/show/mini/1e10q")]
        [InlineData("http://twitpic.com/show/mini/1e10q", "http://twitpic.com/show/mini/1e10q")]
        public void GetImageLocations_GivenTwitPicImageUrl_ReturnsThumbnailLocation(string url, string linkUrl)
        {
            ImageLocations locations = new TwitPicDecoder().GetImageLocations(new Uri(url));

            locations.Thumbnail.ToString().Should().Equal(linkUrl);
        }
    }
}