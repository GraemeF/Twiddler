namespace Twiddler.AcceptanceTests
{
    #region Using Directives

    using TechTalk.SpecFlow;

    using Twiddler.AcceptanceTests.TestEntities;

    #endregion

    [Binding]
    public class EventDefinitions
    {
        [AfterScenario]
        public void AfterScenario()
        {
            TwiddlerApplication application;
            if (ScenarioContext.Current.TryGetValue("Application", out application))
            {
                ScenarioContext.Current.Remove("Application");
                application.Dispose();
            }
        }
    }
}