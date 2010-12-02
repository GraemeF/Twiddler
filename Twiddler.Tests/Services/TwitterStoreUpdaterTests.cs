using Moq;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class TimelineUpdaterTests
    {
        private readonly Mock<IRequestConductor> _fakeRequestConductor = new Mock<IRequestConductor>();
        private readonly Mock<ITimeline> _fakeStore = new Mock<ITimeline>();

        [Fact]
        public void Start__StartsRequestingTweetsForStore()
        {
            TimelineUpdater test = BuildDefaultTestSubject();
            test.Start();

            _fakeRequestConductor.Verify(x => x.Start(_fakeStore.Object));
        }

        private TimelineUpdater BuildDefaultTestSubject()
        {
            return new TimelineUpdater(_fakeRequestConductor.Object, _fakeStore.Object);
        }
    }
}