using Moq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.TweetSharp.Tests
{
    public class TwitterClientTests
    {
        [Fact]
        public void GettingAuthorization_Initially_ReturnsUnknown()
        {
            var test = new TwitterClient(new Mock<ITwitterApplicationCredentials>().Object,
                                         new Mock<IAccessTokenStore>().Object,
                                         x => A.User);

            Assert.Equal(AuthorizationStatus.Unknown, test.AuthorizationStatus);
        }
    }
}