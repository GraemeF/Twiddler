using System;
using TechTalk.SpecFlow;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class ApplicationSteps<TApplication>
        where TApplication : class, new()
    {
        protected static TApplication Application
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("Application"))
                    Launch();

                return ScenarioContext.Current.Get<TApplication>("Application");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            var disposable = Application as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        protected static void Launch()
        {
            var application = new TApplication();
            ScenarioContext.Current.Add("Application", application);
        }
    }
}