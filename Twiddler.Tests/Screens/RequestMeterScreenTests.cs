namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    using Caliburn.Testability.Extensions;

    using NSubstitute;

    using Should.Core.Exceptions;
    using Should.Fluent;

    using Twiddler.Screens;
    using Twiddler.Services.Interfaces;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class RequestMeterScreenTests
    {
        private static readonly DateTime EndOfPeriod = new DateTime(2000, 1, 1);

        private readonly IClock _clock = Substitute.For<IClock>();

        private readonly IRequestLimitStatus _requestStatus = Substitute.For<IRequestLimitStatus>();

        public RequestMeterScreenTests()
        {
            _requestStatus.PeriodEndTime = EndOfPeriod;
            _requestStatus.PeriodDuration.Returns(TimeSpan.FromMinutes(100));
        }

        [Fact]
        public void GettingHourlyLimit__GetsHourlyLimitFromLimitStatus()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            const int hourlyLimit = 350;
            _requestStatus.HourlyLimit = hourlyLimit;

            test.HourlyLimit.Should().Equal(hourlyLimit);
        }

        [Fact]
        public void GettingPeriodDuration__GetsFormattedPeriodDurationFromLimitStatus()
        {
            var duration = new TimeSpan(1, 23, 0);
            _requestStatus.PeriodDuration.Returns(duration);

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.PeriodDuration.Should().Equal("83m");
        }

        [Fact]
        public void GettingRemainingHits__GetsRemainingHitsFromLimitStatus()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            const int remainingHits = 33;
            _requestStatus.RemainingHits = remainingHits;

            test.RemainingHits.Should().Equal(remainingHits);
        }

        [Fact]
        public void GettingRemainingTime_WhenClockHasPassedTheEndTime_ReturnsZero()
        {
            TimeLeftInPeriodIs(-new TimeSpan(0, 4, 0));

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.RemainingTime.Should().Equal("0m");
        }

        [Fact]
        public void GettingRemainingTime__GetsFormattedRemainingTimeFromLimitStatus()
        {
            TimeLeftInPeriodIs(new TimeSpan(0, 4, 11));

            RequestMeterScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.RemainingTime.Should().Equal("5m");
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

            _requestStatus.RemainingHits = remainingHits;
            _requestStatus.HourlyLimit = 100;

            test.UsedHitsFraction.Should().Equal(fraction);
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

            test.UsedTimeFraction.Should().Equal(fraction);
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

            test.PropertyChanged +=
                (sender, args) => { throw new AssertException("No properties should have changed."); };

            PropertyChangesOnRequestStatus("RemainingHits");
        }

        private RequestMeterScreen BuildDefaultTestSubject()
        {
            return new RequestMeterScreen(_requestStatus, _clock);
        }

        private void PropertyChangesOnRequestStatus(string propertyName)
        {
            _requestStatus.PropertyChanged +=
                Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs(propertyName));
        }

        private void TimeLeftInPeriodIs(TimeSpan remainingTime)
        {
            _clock.Now.Returns(EndOfPeriod - remainingTime);
        }
    }
}