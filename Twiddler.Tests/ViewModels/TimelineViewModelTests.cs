namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Caliburn.Micro;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.ViewModels;
    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class TimelineViewModelTests
    {
        private readonly ITimeline _timeline = Substitute.For<ITimeline>();

        private readonly ObservableCollection<ITweet> _tweets = new ObservableCollection<ITweet>();

        public TimelineViewModelTests()
        {
            _timeline.Tweets.Returns(_tweets);
        }

        [Fact]
        public void GettingScreens_WhenThereAreNoTweets_IsEmpty()
        {
            var test = new TimelineViewModel(new Lazy<ITimeline>(() => _timeline), null);

            test.Items.Should().Be.Empty();
        }

        [Fact]
        public void GettingScreens_WhenThereIsATweet_ContainsAScreen()
        {
            var mockScreen = Substitute.For<ITweetScreen>();

            var test = new TimelineViewModel(new Lazy<ITimeline>(() => _timeline), x => mockScreen);
            ((IActivate)test).Activate();

            _tweets.Add(A.Tweet.Build());

            mockScreen.Received().Activate();
            test.Items.Should().Contain.Item(mockScreen);
        }

        [Fact]
        public void SettingSelection_WhenATweetWasSelected_MarksTweetAsRead()
        {
            var mockScreen = Substitute.For<ITweetScreen>();
            var mockCommand = Substitute.For<ICommand>();
            bool commandExecuted = false;

            mockScreen.MarkAsReadCommand.Returns(mockCommand);

            mockCommand.When(x => x.Execute(null)).Do(_ => commandExecuted = true);

            var test = new TimelineViewModel(new Lazy<ITimeline>(() => _timeline), x => mockScreen);
            ((IActivate)test).Activate();

            _tweets.Add(A.Tweet.Build());

            test.Selection = mockScreen;
            test.Selection = null;

            Wait.Until(() => commandExecuted);
        }
    }
}