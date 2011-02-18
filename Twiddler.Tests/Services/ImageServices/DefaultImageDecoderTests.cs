namespace Twiddler.Tests.Services.ImageServices
{
    #region Using Directives

    using System;

    using Should.Fluent;

    using Twiddler.Models;
    using Twiddler.Services.ImageServices;

    using Xunit.Extensions;

    #endregion

    public class DefaultImageDecoderTests
    {
        [Theory]
        [InlineData("http://yfrog.com/0u6mcz")]
        [InlineData("http://graemef.com")]
        public void CanGetImageLocations_GivenAnyUrl_ReturnsTrue(string url)
        {
            BuildTestSubject().CanGetImageLocations(new Uri(url)).Should().Be.True();
        }

        [Theory]
        [InlineData("http://graemef.com/path", "http://graemef.com/favicon.ico")]
        [InlineData("http://graemef.com", "http://graemef.com/favicon.ico")]
        public void GetImageLocations_GivenUrl_ReturnsFavIconLocation(string url, string thumbUrl)
        {
            ImageLocations locations = BuildTestSubject().GetImageLocations(new Uri(url));

            locations.Thumbnail.ToString().Should().Equal(thumbUrl);
        }

        [Theory]
        [InlineData("http://graemef.com/path", "http://graemef.com/path")]
        [InlineData("http://graemef.com", "http://graemef.com/")]
        public void GetImageLocations_GivenUrl_ReturnsLinkLocation(string url, string linkUrl)
        {
            ImageLocations locations = BuildTestSubject().GetImageLocations(new Uri(url));

            locations.Link.ToString().Should().Equal(linkUrl);
        }

        private DefaultImageDecoder BuildTestSubject()
        {
            return new DefaultImageDecoder();
        }
    }
}