namespace Twiddler.AcceptanceTests
{
    #region Using Directives

    using Should.Fluent;

    using TechTalk.SpecFlow;

    #endregion

    [Binding]
    public class AuthorizationStepDefinitions : ApplicationSteps
    {
        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            ScenarioContext.Current.Set(true, "NewStore");
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
            Application.AuthorizationWindow.HasError.Should().Be.True();
        }

        [Then(@"I should be authorized")]
        public void ThenIShouldBeAuthorized()
        {
            Application.AuthorizationStatus.Should().Equal("Authorized");
        }

        [Then(@"I should be unauthorized")]
        public void ThenIShouldBeUnuathorized()
        {
            Application.AuthorizationStatus.Should().Equal("Unauthorized");
        }

        [When(@"I authorize with Twitter")]
        public void WhenIAuthorizeWithTwitter()
        {
            Application.Authorize();
        }
    }
}