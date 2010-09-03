using Moq;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class TwitterClientTests
    {
        [Fact]
        public void GettingAuthorization_Initially_ReturnsUnknown()
        {
            var test = new TwitterClient(new Mock<ICredentialsStore>().Object, x => A.User);

            Assert.Equal(AuthorizationStatus.Unknown, test.AuthorizationStatus);
        }
    }
}