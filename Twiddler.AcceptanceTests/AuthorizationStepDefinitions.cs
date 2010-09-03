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
        private bool _newStore;

        private void EnsureTwiddlerHasBeenStarted()
        {
            if (_twiddler == null)
                _twiddler = TwiddlerApplication.Launch(_newStore);
        }

        [BeforeScenario]
        public void ResetStartupParameters()
        {
            _newStore = true;
        }

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
            EnsureTwiddlerHasBeenStarted();

            Assert.Equal("Unauthorized", _twiddler.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            _newStore = true;
        }

        [When(@"I click the Authorize button")]
        public void WhenIClickTheAuthorizeButton()
        {
            EnsureTwiddlerHasBeenStarted();

            ScenarioContext.Current.Pending();
        }

        [Then(@"the authorization window should be displayed")]
        public void ThenTheAuthorizationWindowShouldBeDisplayed()
        {
            EnsureTwiddlerHasBeenStarted();

            ScenarioContext.Current.Pending();
        }
    }
}