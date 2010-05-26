using Moq;
using Twiddler.Screens;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class RequestMeterTests
    {
        private readonly Mock<IRequestStatus> _fakeRequestStatus = new Mock<IRequestStatus>();

        [Fact]
        public void GettingHourlyLimit__GetsHourlyLimitFromLimitStatus()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            const int hourlyLimit = 350;
            _fakeRequestStatus.Setup(x => x.HourlyLimit).Returns(hourlyLimit);

            Assert.Equal(hourlyLimit, test.HourlyLimit);
        }

        [Fact]
        public void GettingRemainingHits__GetsRemainingHitsFromLimitStatus()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            const int remainingHits = 33;
            _fakeRequestStatus.Setup(x => x.RemainingHits).Returns(remainingHits);

            Assert.Equal(remainingHits, test.RemainingHits);
        }

        private RequestMeterScreen BuildDefaultTestSubject()
        {
            return new RequestMeterScreen(_fakeRequestStatus.Object);
        }
    }
}