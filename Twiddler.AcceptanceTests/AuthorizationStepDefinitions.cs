using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class AuthorizationStepDefinitions : TwiddlerStepDefinitions
    {
        [Then(@"I should be unauthorized")]
        public void ThenIShouldBeUnuathorized()
        {
            EnsureTwiddlerHasBeenStarted();

            Assert.Equal("Unauthorized", Twiddler.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [Then(@"I should be authorized")]
        public void ThenIShouldBeAuthorized()
        {
            EnsureTwiddlerHasBeenStarted();

            Assert.Equal("Authorized", Twiddler.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [When(@"I authorize with Twitter")]
        public void WhenIAuthorizeWithTwitter()
        {
            EnsureTwiddlerHasBeenStarted();

            Twiddler.Authorize();
        }

        [Given(@"Twitter is unavailable")]
        public void GivenTwitterIsUnavailable()
        {
            Twitter.Dispose();
            Twitter = null;
        }

        [Then(@"authorization should fail")]
        public void ThenAuthorizationShouldFail()
        {
            Assert.True(Twiddler.AuthorizationWindow.HasError);
        }
    }
}