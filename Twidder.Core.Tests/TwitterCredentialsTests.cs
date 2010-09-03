using Twiddler.Core.Models;
using Xunit;

namespace Twidder.Core.Tests
{
    public class TwitterCredentialsTests
    {
        [Fact]
        public void GettingAreValid_WhenCredentialsAreComplete_ReturnsTrue()
        {
            var test = new TwitterCredentials("id", "accessToken", "accessTokenSecret");
            Assert.True(test.AreValid);
        }

        [Fact]
        public void GettingAreValid_WhenAccessTokenIsMissing_ReturnsFalse()
        {
            var test = new TwitterCredentials("id", "", "accessTokenSecret");
            Assert.False(test.AreValid);
        }
    }
}