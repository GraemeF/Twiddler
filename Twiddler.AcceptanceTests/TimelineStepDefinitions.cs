﻿using TechTalk.SpecFlow;

namespace Twiddler.AcceptanceTests
{
    [Binding]
    public class TimelineStepDefinitions
    {
        [Given(@"I have an empty Timeline")]
        public void GivenIHaveAnEmptyTimeline()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"all Tweets should be in descending order of time")]
        public void ThenAllTweetsShouldBeInDescendingOrderOfTime()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"3 uninteresting Tweets are retrieved")]
        public void When_3UninterestingTweetsAreRetrieved()
        {
            ScenarioContext.Current.Pending();
        }
    }
}