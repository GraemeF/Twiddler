using System;
using TechTalk.SpecFlow;
using Twiddler.AcceptanceTests.TestEntities;
using Xunit;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class AuthorizationStepDefinitions
    {
        private bool _newStore;
        private TwiddlerApplication _twiddler;

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

        [Then(@"I should be unauthorized")]
        public void ThenIShouldBeUnuathorized()
        {
            EnsureTwiddlerHasBeenStarted();

            Assert.Equal("Unauthorized", _twiddler.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            _newStore = true;
        }

        [Then(@"I should be authorized")]
        public void ThenIShouldBeAuthorized()
        {
            EnsureTwiddlerHasBeenStarted();

            Assert.Equal("Authorized", _twiddler.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [When(@"I authorize with Twitter")]
        public void WhenIAuthorizeWithTwitter()
        {
            EnsureTwiddlerHasBeenStarted();

            _twiddler.Authorize();
        }
    }
}