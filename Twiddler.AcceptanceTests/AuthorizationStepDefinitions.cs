using TechTalk.SpecFlow;
using Twiddler.AcceptanceTests.TestEntities;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class AuthorizationStepDefinitions
    {
        private TwiddlerApplication _twiddler;

        [BeforeScenario]
        public void StartApplication()
        {
            _twiddler = TwiddlerApplication.Launch();
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

        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the authorization status should show I am unauthorized")]
        public void ThenTheAuthorizationStatusShouldShowIAmUnuathorized()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I start the application")]
        public void WhenIStartTheApplication()
        {
            ScenarioContext.Current.Pending();
        }
    }
}