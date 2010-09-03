using Moq;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class TwitterStoreUpdaterTests
    {
        private readonly Mock<IRequestConductor> _fakeRequestConductor = new Mock<IRequestConductor>();
        private readonly Mock<ITweetStore> _fakeStore = new Mock<ITweetStore>();

        [Fact]
        public void Start__StartsRequestingTweetsForStore()
        {
            TwitterStoreUpdater test = BuildDefaultTestSubject();
            test.Start();

            _fakeRequestConductor.Verify(x => x.Start(_fakeStore.Object));
        }

        private TwitterStoreUpdater BuildDefaultTestSubject()
        {
            return new TwitterStoreUpdater(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}