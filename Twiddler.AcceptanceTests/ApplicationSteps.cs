namespace Twiddler.AcceptanceTests
{
    #region Using Directives

    using System;

    using TechTalk.SpecFlow;

    using Twiddler.AcceptanceTests.TestEntities;

    #endregion

    [Binding]
    public class ApplicationSteps
    {
        protected static TwitterService Twitter;

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

        [Given(@"I have not previously authorized")]
        public void GivenIHaveNotPreviouslyAuthorized()
        {
            ScenarioContext.Current.Set(true, "NewStore");
        }

        [BeforeScenario]
        public void StartTwitterService()
        {
            Twitter = new TwitterService();
            Twitter.Start();
        }

        protected static void Launch()
        {
            var application = new TwiddlerApplication();
            ScenarioContext.Current.Add("Application", application);
        }
    }
}