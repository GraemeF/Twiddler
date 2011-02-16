namespace Twiddler.Tests.Services.ImageServices
{
    #region Using Directives

    using System;

    using Should.Fluent;

    using Twiddler.Models;
    using Twiddler.Services.ImageServices;

    using Xunit.Extensions;

    #endregion

    public class YFrogDecoderTests
    {
        [Theory]
        [InlineData("http://some.other.com/")]
        [InlineData("http://yfrog.com/")]
        public void CanGetImageLocations_GivenOtherUrl_ReturnsFalse(string url)
        {
            new YFrogDecoder().CanGetImageLocations(new Uri(url)).Should().Be.False();
        }

        [Theory]
        [InlineData("http://yfrog.com/0u6mcz")]
        [InlineData("http://yfrog.ru/0u6mcz")]
        [InlineData("http://yfrog.com.tr/0u6mcz")]
        [InlineData("http://yfrog.it/0u6mcz")]
        [InlineData("http://yfrog.fr/0u6mcz")]
        [InlineData("http://yfrog.co.il/0u6mcz")]
        [InlineData("http://yfrog.co.uk/0u6mcz")]
        [InlineData("http://yfrog.com.pl/0u6mcz")]
        [InlineData("http://yfrog.pl/0u6mcz")]
        [InlineData("http://yfrog.eu/0u6mcz")]
        public void CanGetImageLocations_GivenYFrogImageUrl_ReturnsTrue(string url)
        {
            new YFrogDecoder().CanGetImageLocations(new Uri(url)).Should().Be.True();
        }

        [Theory]
        [InlineData("http://yfrog.com/0u6mcd")]
        [InlineData("http://yfrog.com/0u6mcs")]
        public void CanGetImageLocations_GivenYFrogUrlOfOtherContentType_ReturnsFalse(string url)
        {
            new YFrogDecoder().CanGetImageLocations(new Uri(url)).Should().Be.False();
        }

        [Theory]
        [InlineData("http://yfrog.com/0u6mcz", "http://yfrog.com/0u6mcz")]
        public void GetImageLocations_GivenYFrogImageUrl_ReturnsLinkLocation(string url, string linkUrl)
        {
            ImageLocations locations = new YFrogDecoder().GetImageLocations(new Uri(url));

            locations.Link.ToString().Should().Equal(linkUrl);
        }

        [Theory]
        [InlineData("http://yfrog.com/0u6mcz", "http://yfrog.com/0u6mcz.th.jpg")]
        public void GetImageLocations_GivenYFrogImageUrl_ReturnsThumbnailLocation(string url, string linkUrl)
        {
            ImageLocations locations = new YFrogDecoder().GetImageLocations(new Uri(url));

            locations.Thumbnail.ToString().Should().Equal(linkUrl);
        }
    }
}