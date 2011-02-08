namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System;

    using Twiddler.Services;

    using Xunit;

    #endregion

    public class ClockTests
    {
        [Fact]
        public void GettingNow__ReturnsCurrentDateAndTimeInLocalTimeZone()
        {
            DateTime beforeNow = DateTime.Now;
            DateTime clockNow = new Clock().Now;
            DateTime afterNow = DateTime.Now;

            Assert.InRange(clockNow, beforeNow, afterNow);
        }
    }
}