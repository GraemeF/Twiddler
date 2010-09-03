using System;
using TechTalk.SpecFlow;
using Twiddler.AcceptanceTests.TestEntities;
using Xunit;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class AuthorizationStepDefinitions
    {
        private TwiddlerApplication _twiddler;

        [AfterScenario]
        public void StopApplication()
        {
            if (_twiddler != null)
            {
                _twiddler.Dispose();
                _twiddler = null;
            }
        }

        [Then(@"the authorization status should show I am unauthorized")]
        public void ThenTheAuthorizationStatusShouldShowIAmUnuathorized()
        {
            Assert.Equal("Unauthorized", _twiddler.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [When(@"I start the application")]
        public void WhenIStartTheApplication()
        {
            _twiddler = TwiddlerApplication.Launch();
        }
    }
}