using TechTalk.SpecFlow;
using Twiddler.AcceptanceTests.TestEntities;

namespace Twiddler.AcceptanceTests
{
    public class TwiddlerStepDefinitions
    {
        protected static bool NewStore;
        protected static TwiddlerApplication Twiddler;
        protected static TwitterService Twitter;

        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            NewStore = true;
        }

        protected void EnsureTwiddlerHasBeenStarted()
        {
            if (Twiddler == null)
                Twiddler = TwiddlerApplication.Launch(NewStore);
        }

        [BeforeScenario]
        public void ResetStartupParameters()
        {
            NewStore = true;
        }

        [BeforeScenario]
        public void StartTwitterService()
        {
            Twitter = new TwitterService();
            Twitter.Start();
        }

        [AfterScenario]
        public void StopApplication()
        {
            if (Twiddler != null)
            {
                Twiddler.Dispose();
                Twiddler = null;
            }
        }
    }
}