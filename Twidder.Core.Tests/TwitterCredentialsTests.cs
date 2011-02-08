namespace Twidder.Core.Tests
{
    #region Using Directives

    using Twiddler.Core.Models;

    using Xunit;

    #endregion

    public class TwitterCredentialsTests
    {
        [Fact]
        public void GettingAreValid_WhenAccessTokenIsMissing_ReturnsFalse()
        {
            var test = new AccessToken("id", string.Empty, "accessTokenSecret");
            Assert.False(test.IsValid);
        }

        [Fact]
        public void GettingAreValid_WhenCredentialsAreComplete_ReturnsTrue()
        {
            var test = new AccessToken("id", "accessToken", "accessTokenSecret");
            Assert.True(test.IsValid);
        }
    }
}