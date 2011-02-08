namespace Twiddler.AcceptanceTests
{
    #region Using Directives

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

        private static void Launch()
        {
            var application = new TwiddlerApplication();
            ScenarioContext.Current.Add("Application", application);
        }
    }
}