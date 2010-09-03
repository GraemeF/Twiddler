using Twiddler.Core.Models;
using Xunit;

namespace Twidder.Core.Tests
{
    public class TwitterCredentialsTests
    {
        [Fact]
        public void GettingAreValid_WhenCredentialsAreComplete_ReturnsTrue()
        {
            var test = new AccessToken("id", "accessToken", "accessTokenSecret");
            Assert.True(test.IsValid);
        }

        [Fact]
        public void GettingAreValid_WhenAccessTokenIsMissing_ReturnsFalse()
        {
            var test = new AccessToken("id", "", "accessTokenSecret");
            Assert.False(test.IsValid);
        }
    }
}