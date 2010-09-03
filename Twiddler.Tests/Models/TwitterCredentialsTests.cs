using Twiddler.Models;
using Xunit;

namespace Twiddler.Tests.Models
{
    public class TwitterCredentialsTests
    {
        [Fact]
        public void GettingAreValid_WhenCredentialsAreComplete_ReturnsTrue()
        {
            var test = new TwitterCredentials("consumerKey", "consumerSecret", "accessToken", "accessTokenSecret");
            Assert.True(test.AreValid);
        }

        [Fact]
        public void GettingAreValid_WhenAccessTokenIsMissing_ReturnsFalse()
        {
            var test = new TwitterCredentials("consumerKey", "consumerSecret", "", "accessTokenSecret");
            Assert.False(test.AreValid);
        }
    }
}