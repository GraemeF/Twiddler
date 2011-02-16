namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using NSubstitute;

    using Twiddler.Core.Models;
    using Twiddler.Screens;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class TimelineScreenTests
    {
        private readonly ITimeline _timeline = Substitute.For<ITimeline>();

        private readonly ObservableCollection<ITweet> _tweets = new ObservableCollection<ITweet>();

        public TimelineScreenTests()
        {
            _timeline.Tweets.Returns(_tweets);
        }

        [Fact]
        public void GettingScreens_WhenThereAreNoTweets_IsEmpty()
        {
            var test = new TimelineScreen(new Lazy<ITimeline>(() => _timeline), null);

            Assert.Empty(test.Screens);
        }

        [Fact]
        public void GettingScreens_WhenThereIsATweet_ContainsAScreen()
        {
            var mockScreen = Substitute.For<ITweetScreen>();

            var test = new TimelineScreen(new Lazy<ITimeline>(() => _timeline), x => mockScreen);
            test.Initialize();

            _tweets.Add(A.Tweet.Build());

            Assert.Contains(mockScreen, test.Screens);
            mockScreen.Received().Initialize();
        }

        [Fact]
        public void SettingSelection_WhenATweetWasSelected_MarksTweetAsRead()
        {
            var mockScreen = Substitute.For<ITweetScreen>();
            var mockCommand = Substitute.For<ICommand>();
            bool commandExecuted = false;

            mockScreen.MarkAsReadCommand.Returns(mockCommand);

            mockCommand.When(x => x.Execute(null)).Do(_ => commandExecuted = true);

            var test = new TimelineScreen(new Lazy<ITimeline>(() => _timeline), x => mockScreen);
            test.Initialize();

            _tweets.Add(A.Tweet.Build());

            test.Selection = mockScreen;
            test.Selection = null;

            Wait.Until(() => commandExecuted);
        }
    }
}