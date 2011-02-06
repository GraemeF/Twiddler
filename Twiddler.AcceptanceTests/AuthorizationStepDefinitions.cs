using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class AuthorizationStepDefinitions : ApplicationSteps
    {
        [Then(@"I should be unauthorized")]
        public void ThenIShouldBeUnuathorized()
        {
            Assert.Equal("Unauthorized", Application.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [Then(@"I should be authorized")]
        public void ThenIShouldBeAuthorized()
        {
            Assert.Equal("Authorized", Application.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [When(@"I authorize with Twitter")]
        public void WhenIAuthorizeWithTwitter()
        {
            Application.Authorize();
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
            Assert.True(Application.AuthorizationWindow.HasError);
        }
    }
}