using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using Moq;
using Twiddler.Core.Models;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TimelineScreenTests
    {
        private readonly Mock<ITimeline> _fakeTimeline = new Mock<ITimeline>();
        private readonly ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();

        public TimelineScreenTests()
        {
            _fakeTimeline.Setup(x => x.Tweets).Returns(_tweets);
        }

        [Fact]
        public void GettingScreens_WhenThereAreNoTweets_IsEmpty()
        {
            var test = new TimelineScreen(new Lazy<ITimeline>(() => _fakeTimeline.Object), null);

            Assert.Empty(test.Screens);
        }

        [Fact]
        public void GettingScreens_WhenThereIsATweet_ContainsAScreen()
        {
            var mockScreen = new Mock<ITweetScreen>();

            var test = new TimelineScreen(new Lazy<ITimeline>(() => _fakeTimeline.Object), x => mockScreen.Object);
            test.Initialize();

            _tweets.Add(New.Tweet);

            Assert.Contains(mockScreen.Object, test.Screens);
            mockScreen.Verify(x => x.Initialize());
        }

        [Fact]
        public void SettingSelection_WhenATweetWasSelected_MarksTweetAsRead()
        {
            var mockScreen = new Mock<ITweetScreen>();
            var mockCommand = new Mock<ICommand>();
            mockScreen.
                Setup(x => x.MarkAsReadCommand).
                Returns(mockCommand.Object);

            var test = new TimelineScreen(new Lazy<ITimeline>(() => _fakeTimeline.Object), x => mockScreen.Object);
            test.Initialize();

            _tweets.Add(New.Tweet);

            test.Selection = mockScreen.Object;
            test.Selection = null;
            Thread.Sleep(1000);

            mockCommand.Verify(x => x.Execute(null));
        }
    }
}