namespace Twiddler.AcceptanceTests
{
    #region Using Directives

    using System;

    using Should.Core.Assertions;
    using Should.Fluent;

    using TechTalk.SpecFlow;

    #endregion

    [Binding]
    public class AuthorizationStepDefinitions : ApplicationSteps
    {
        [Given(@"Twitter is unavailable")]
        public void GivenTwitterIsUnavailable()
        {
            Twitter.Dispose();
            Twitter = null;
        }

        [Then(@"authorization should fail")]
        public void ThenAuthorizationShouldFail()
        {
            Application.AuthorizationWindow.HasError.Should().Be.True();
        }

        [Then(@"I should be authorized")]
        public void ThenIShouldBeAuthorized()
        {
            Assert.Equal("Authorized", Application.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [Then(@"I should be unauthorized")]
        public void ThenIShouldBeUnuathorized()
        {
            Assert.Equal("Unauthorized", Application.AuthorizationStatus, StringComparer.CurrentCultureIgnoreCase);
        }

        [When(@"I authorize with Twitter")]
        public void WhenIAuthorizeWithTwitter()
        {
            Application.Authorize();
        }
    }
}