using Moq;
using Twiddler.Models;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class UpdatingTwitterStoreTests
    {
        private readonly Mock<IRequestConductor> _fakeRequestConductor = new Mock<IRequestConductor>();
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void AddTweet__AddsTweetToStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeStore.Verify(x => x.AddTweet(tweet));
        }

        [Fact]
        public void GetTweet__GetsTweetFromStore()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            Tweet tweet = New.Tweet;
            _fakeStore.Setup(x => x.GetTweet(tweet.Id)).Returns(tweet);

            Assert.Same(tweet, test.GetTweet(tweet.Id));
        }

        private UpdatingTwitterStore BuildDefaultTestSubject()
        {
            return new UpdatingTwitterStore(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}