namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    using Caliburn.Micro;

    using NSubstitute;

    using Should.Core.Exceptions;
    using Should.Fluent;

    using Twiddler.ViewModels;
    using Twiddler.Services.Interfaces;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class RequestMeterViewModelTests
    {
        private static readonly DateTime EndOfPeriod = new DateTime(2000, 1, 1);

        private readonly IClock _clock = Substitute.For<IClock>();

        private readonly IRequestLimitStatus _requestStatus = Substitute.For<IRequestLimitStatus>();

        public RequestMeterViewModelTests()
        {
            _requestStatus.PeriodEndTime = EndOfPeriod;
            _requestStatus.PeriodDuration.Returns(TimeSpan.FromMinutes(100));
        }

        [Fact]
        public void GettingHourlyLimit__GetsHourlyLimitFromLimitStatus()
        {
            RequestMeterViewModel test = BuildDefaultTestSubject();

            const int hourlyLimit = 350;
            _requestStatus.HourlyLimit = hourlyLimit;

            test.HourlyLimit.Should().Equal(hourlyLimit);
        }

        [Fact]
        public void GettingPeriodDuration__GetsFormattedPeriodDurationFromLimitStatus()
        {
            var duration = new TimeSpan(1, 23, 0);
            _requestStatus.PeriodDuration.Returns(duration);

            RequestMeterViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.PeriodDuration.Should().Equal("83m");
        }

        [Fact]
        public void GettingRemainingHits__GetsRemainingHitsFromLimitStatus()
        {
            RequestMeterViewModel test = BuildDefaultTestSubject();

            const int remainingHits = 33;
            _requestStatus.RemainingHits = remainingHits;

            test.RemainingHits.Should().Equal(remainingHits);
        }

        [Fact]
        public void GettingRemainingTime_WhenClockHasPassedTheEndTime_ReturnsZero()
        {
            TimeLeftInPeriodIs(-new TimeSpan(0, 4, 0));

            RequestMeterViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.RemainingTime.Should().Equal("0m");
        }

        [Fact]
        public void GettingRemainingTime__GetsFormattedRemainingTimeFromLimitStatus()
        {
            TimeLeftInPeriodIs(new TimeSpan(0, 4, 11));

            RequestMeterViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

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
            RequestMeterViewModel test = BuildDefaultTestSubject();

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
            RequestMeterViewModel test = BuildDefaultTestSubject();

            TimeLeftInPeriodIs(TimeSpan.FromMinutes(remainingMinutes));

            test.UsedTimeFraction.Should().Equal(fraction);
        }

        [Fact]
        public void HourlyLimit_WhenChangedOnLimitStatus_RaisesPropertyChanged()
        {
            RequestMeterViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.HourlyLimit, 
                                                       () => PropertyChangesOnRequestStatus("HourlyLimit"));
        }

        [Fact]
        public void RemainingHits_WhenChangedOnLimitStatus_RaisesPropertyChanged()
        {
            RequestMeterViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.RemainingHits, 
                                                       () => PropertyChangesOnRequestStatus("RemainingHits"));
        }

        [Fact]
        public void Deactivate__UnsubscribesFromRequestStatusChanges()
        {
            RequestMeterViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();
            ((IDeactivate)test).Deactivate(true);

            test.PropertyChanged +=
                (sender, args) => { throw new AssertException("No properties should have changed."); };

            PropertyChangesOnRequestStatus("RemainingHits");
        }

        private RequestMeterViewModel BuildDefaultTestSubject()
        {
            return new RequestMeterViewModel(_requestStatus, _clock);
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