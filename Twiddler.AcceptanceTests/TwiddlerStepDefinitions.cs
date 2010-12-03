using TechTalk.SpecFlow;
using Twiddler.AcceptanceTests.TestEntities;

namespace Twiddler.AcceptanceTests
{
    public class TwiddlerStepDefinitions : ApplicationSteps<TwiddlerApplication>
    {
        protected static TwitterService Twitter;

        [BeforeScenario]
        public void StartTwitterService()
        {
            Twitter = new TwitterService();
            Twitter.Start();
        }

        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            ScenarioContext.Current.Set(true, "NewStore");
        }
    }
}