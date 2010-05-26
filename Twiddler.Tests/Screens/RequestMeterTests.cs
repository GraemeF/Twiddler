using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class RequestMeterTests
    {
        [Fact]
        public void GettingHourlyLimit__GetsHourlyLimitFromLimitStatus()
        {
            var test = BuildDefaultTestSubject();
        }

        private RequestMeter BuildDefaultTestSubject()
        {
            return new RequestMeter();
        }
    }
}
