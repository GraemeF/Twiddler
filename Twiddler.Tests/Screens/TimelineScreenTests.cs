using System.Collections.Generic;
using Moq;
using Twiddler.Models.Interfaces;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TimelineScreenTests
    {
        private readonly Mock<ITimeline> _fakeTimeline = new Mock<ITimeline>();
        private readonly Subject<ITweet> _tweetObservable = new Subject<ITweet>();

        public TimelineScreenTests()
        {
            _fakeTimeline.Setup(x => x.Tweets).Returns(_tweetObservable);
        }

        [Fact]
        public void GettingScreens_WhenThereAreNoTweets_IsEmpty()
        {
            var test = new TimelineScreen(_fakeTimeline.Object, null);

            Assert.Empty(test.Screens);
        }

        [Fact]
        public void GettingScreens_WhenThereIsATweet_ContainsAScreen()
        {
            var mockScreen = new Mock<ITweetScreen>();

            var test = new TimelineScreen(_fakeTimeline.Object, x => mockScreen.Object);
            test.Initialize();

            _tweetObservable.OnNext(New.Tweet);

            Assert.Contains(mockScreen.Object, test.Screens);
            mockScreen.Verify(x => x.Initialize());
        }

        [Fact]
        public void GettingScreens_WhenShutdownBeforeTweetArrives_IsEmpty()
        {
            var test = new TimelineScreen(_fakeTimeline.Object, null);
            test.Initialize();
            test.Shutdown();

            _tweetObservable.OnNext(New.Tweet);

            Assert.Empty(test.Screens);
        }
    }
}