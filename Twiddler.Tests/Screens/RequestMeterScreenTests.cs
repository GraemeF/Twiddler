namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    using Caliburn.Testability.Extensions;

    using Moq;

    using Twiddler.Screens;
    using Twiddler.Services.Interfaces;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class RequestMeterScreenTests
    {
        private static readonly DateTime EndOfPeriod = new DateTime(2000, 1, 1);

        private readonly Mock<IClock> _fakeClock = new Mock<IClock>();

        private readonly Mock<IRequestLimitStatus> _fakeRequestStatus = new Mock<IRequestLimitStatus>();

        public RequestMeterScreenTests()
        {
            _fakeRequestStatus.SetupAllProperties();
            _fakeRequestStatus.Object.PeriodEndTime = EndOfPeriod;
            _fakeRequestStatus.Setup(x => x.PeriodDuration).Returns(TimeSpan.FromMinutes(100));
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
            var duration = new TimeSpan(1, 23, 0);
            _fakeRequestStatus.
                Setup(x => x.PeriodDuration).
                Returns(duration);

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal("83m", test.PeriodDuration);
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
        public void GettingRemainingTime_WhenClockHasPassedTheEndTime_ReturnsZero()
        {
            TimeLeftInPeriodIs(-new TimeSpan(0, 4, 0));

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal("0m", test.RemainingTime);
        }

        [Fact]
        public void GettingRemainingTime__GetsFormattedRemainingTimeFromLimitStatus()
        {
            TimeLeftInPeriodIs(new TimeSpan(0, 4, 11));

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal("5m", test.RemainingTime);
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

            TimeLeftInPeriodIs(TimeSpan.FromMinutes(remainingMinutes));

            Assert.Equal(fraction, test.UsedTimeFraction);
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
        public void Shutdown__UnsubscribesFromRequestStatusChanges()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            test.Initialize();
            test.Shutdown();

            test.PropertyChanged += (sender, args) => Assert.True(false);

            PropertyChangesOnRequestStatus("RemainingHits");
        }

        private RequestMeterScreen BuildDefaultTestSubject()
        {
            return new RequestMeterScreen(_fakeRequestStatus.Object, _fakeClock.Object);
        }

        private void PropertyChangesOnRequestStatus(string propertyName)
        {
            _fakeRequestStatus.Raise(x => x.PropertyChanged += null, 
                                     new PropertyChangedEventArgs(propertyName));
        }

        private void TimeLeftInPeriodIs(TimeSpan remainingTime)
        {
            _fakeClock.
                Setup(x => x.Now).
                Returns(EndOfPeriod - remainingTime);
        }
    }
}