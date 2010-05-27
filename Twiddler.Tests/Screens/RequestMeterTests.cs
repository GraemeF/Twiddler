using System.ComponentModel;
using Caliburn.Testability.Extensions;
using Moq;
using Twiddler.Screens;
using Twiddler.Services.Interfaces;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Screens
{
    public class RequestMeterTests
    {
        private readonly Mock<IRequestLimitStatus> _fakeRequestStatus = new Mock<IRequestLimitStatus>();

        public RequestMeterTests()
        {
            _fakeRequestStatus.SetupAllProperties();
        }

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

        [Fact]
        public void RemainingHits_WhenChangedOnLimitStatus_RaisesPropertyChanged()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.RemainingHits).
                When(() => PropertyChangesOnRequestStatus("RemainingHits"));
        }

        [Fact]
        public void HourlyLimit_WhenChangedOnLimitStatus_RaisesPropertyChanged()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.HourlyLimit).
                When(() => PropertyChangesOnRequestStatus("HourlyLimit"));
        }

        [Theory]
        [InlineData(-0, 1f)]
        [InlineData(-1, 1f)]
        [InlineData(75, 0.25f)]
        [InlineData(25, 0.75f)]
        [InlineData(100, 0f)]
        [InlineData(101, 0f)]
        public void GettingUsedHitsFraction__ReturnsFractionOfHourlyLimit(int remainingHits, float fraction)
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            _fakeRequestStatus.Object.RemainingHits = remainingHits;
            _fakeRequestStatus.Object.HourlyLimit = 100;

            Assert.Equal(fraction, test.UsedHitsFraction);
        }

        private void PropertyChangesOnRequestStatus(string propertyName)
        {
            _fakeRequestStatus.Raise(x => x.PropertyChanged += null,
                                     new PropertyChangedEventArgs(propertyName));
        }

        private RequestMeterScreen BuildDefaultTestSubject()
        {
            return new RequestMeterScreen(_fakeRequestStatus.Object);
        }
    }
}