using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Caliburn.Testability.Extensions;
using Moq;
using Twiddler.Screens;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class RequestMeterTests
    {
        private readonly Mock<IRequestLimitStatus> _fakeRequestStatus = new Mock<IRequestLimitStatus>();

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

            AssertPropertyChangedIsRaised(test,
                                          x => x.RemainingHits,
                                          "RemainingHits");
        }

        [Fact]
        public void RemainingTime_WhenChangedOnLimitStatus_RaisesPropertyChanged()
        {
            RequestMeterScreen test = BuildDefaultTestSubject();

            AssertPropertyChangedIsRaised(test,
                                          x => x.RemainingTime,
                                          "RemainingTime");
        }

        private void AssertPropertyChangedIsRaised(RequestMeterScreen test,
                                                   Expression<Func<RequestMeterScreen, object>> property,
                                                   string propertyName)
        {
            test.
                AssertThatChangeNotificationIsRaisedBy(property).
                When(() => _fakeRequestStatus.Raise(x => x.PropertyChanged += null,
                                                    new PropertyChangedEventArgs(propertyName)));
        }

        private RequestMeterScreen BuildDefaultTestSubject()
        {
            return new RequestMeterScreen(_fakeRequestStatus.Object);
        }
    }
}