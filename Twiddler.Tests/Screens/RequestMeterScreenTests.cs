using System;
using System.ComponentModel;
using Caliburn.Testability.Extensions;
using Moq;
using TweetSharp.Extensions;
using Twiddler.Screens;
using Twiddler.Services.Interfaces;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Screens
{
    public class RequestMeterScreenTests
    {
        private static readonly DateTime EndOfPeriod = new DateTime(2000, 1, 1);
        private readonly Mock<IClock> _fakeClock = new Mock<IClock>();
        private readonly Mock<IRequestLimitStatus> _fakeRequestStatus = new Mock<IRequestLimitStatus>();

        public RequestMeterScreenTests()
        {
            _fakeRequestStatus.SetupAllProperties();
            _fakeRequestStatus.Object.PeriodEndTime = EndOfPeriod;
            _fakeRequestStatus.Setup(x => x.PeriodDuration).Returns(100.Minutes());
        }

        [Fact]
        public void Shutdown__UnsubscribesFromRequestStatusChanges()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            test.Initialize();
            test.Shutdown();

            test.PropertyChanged += (sender, args) => Assert.True(false);

            PropertyChangesOnRequestStatus("RemainingHits");
        }

        [Fact]
        public void GettingHourlyLimit__GetsHourlyLimitFromLimitStatus()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            const int hourlyLimit = 350;
            _fakeRequestStatus.Object.HourlyLimit = hourlyLimit;

            Assert.Equal(hourlyLimit, test.HourlyLimit);
        }

        [Fact]
        public void GettingPeriodDuration__GetsFormattedPeriodDurationFromLimitStatus()
        {
            TimeSpan duration = 1.Hour() + 23.Minutes();
            _fakeRequestStatus.
                Setup(x => x.PeriodDuration).
                Returns(duration);

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal("83m", test.PeriodDuration);
        }

        [Fact]
        public void GettingRemainingTime__GetsFormattedRemainingTimeFromLimitStatus()
        {
            TimeLeftInPeriodIs(4.Minutes() + 11.Seconds());

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal("5m", test.RemainingTime);
        }

        [Fact]
        public void GettingRemainingTime_WhenClockHasPassedTheEndTime_ReturnsZero()
        {
            TimeLeftInPeriodIs(-4.Minutes());

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal("0m", test.RemainingTime);
        }

        [Fact]
        public void GettingRemainingHits__GetsRemainingHitsFromLimitStatus()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            const int remainingHits = 33;
            _fakeRequestStatus.Object.RemainingHits = remainingHits;

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

        [Theory]
        [InlineData(0, 1f)]
        [InlineData(-1, 1f)]
        [InlineData(75, 0.25f)]
        [InlineData(25, 0.75f)]
        [InlineData(100, 0f)]
        [InlineData(101, 0f)]
        public void GettingUsedTimeFraction__ReturnsFractionOfPeriodDuration(int remainingMinutes, float fraction)
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            TimeLeftInPeriodIs(remainingMinutes.Minutes());

            Assert.Equal(fraction, test.UsedTimeFraction);
        }

        private void TimeLeftInPeriodIs(TimeSpan remainingTime)
        {
            _fakeClock.
                Setup(x => x.Now).
                Returns(EndOfPeriod - remainingTime);
        }

        private void PropertyChangesOnRequestStatus(string propertyName)
        {
            _fakeRequestStatus.Raise(x => x.PropertyChanged += null,
                                     new PropertyChangedEventArgs(propertyName));
        }

        private RequestMeterScreen BuildDefaultTestSubject()
        {
            return new RequestMeterScreen(_fakeRequestStatus.Object, _fakeClock.Object);
        }
    }
}