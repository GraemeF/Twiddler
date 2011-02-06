using System;
using TechTalk.SpecFlow;
using Twiddler.AcceptanceTests.TestEntities;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class ApplicationSteps
    {
        protected static TwiddlerApplication Application
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("Application"))
                    Launch();

                return ScenarioContext.Current.Get<TwiddlerApplication>("Application");
            }
        }

        [AfterScenario]
        public static void AfterTestRun()
        {
            var disposable = Application as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        protected static void Launch()
        {
            var application = new TwiddlerApplication();
            ScenarioContext.Current.Add("Application", application);
        }
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