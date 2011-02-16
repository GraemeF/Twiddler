namespace Twidder.Core.Tests
{
    #region Using Directives

    using Should.Fluent;

    using Twiddler.Core.Models;

    using Xunit;

    #endregion

    public class TwitterCredentialsTests
    {
        [Fact]
        public void GettingAreValid_WhenAccessTokenIsMissing_ReturnsFalse()
        {
            var test = new AccessToken("id", string.Empty, "accessTokenSecret");
            test.IsValid.Should().Be.False();
        }

        [Fact]
        public void GettingAreValid_WhenCredentialsAreComplete_ReturnsTrue()
        {
            var test = new AccessToken("id", "accessToken", "accessTokenSecret");
            test.IsValid.Should().Be.True();
        }
    }
}