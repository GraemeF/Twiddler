namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;
    using System.Windows;

    using Caliburn.Testability.Extensions;

    using NSubstitute;

    using Twiddler.Commands;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Screens;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class TweetScreenTests
    {
        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory =
            Substitute.For<ILinkThumbnailScreenFactory>();

        private readonly ILinkThumbnailScreen _linkThumbnailScreen = Substitute.For<ILinkThumbnailScreen>();

        private readonly ITweetRating _tweetRating = Substitute.For<ITweetRating>();

        private readonly ITweet _tweet = A.Tweet.Build();

        [Fact]
        public void GettingCreatedDate__ReturnsCreatedDate()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.CreatedDate, test.CreatedDate);
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Id, test.Id);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsAReply_IsALoadingTweetScreen()
        {
            var mockScreen = Substitute.For<ILoadingTweetScreen>();

            var test = new TweetScreen(A.Tweet.InReplyTo("4").Build(), 
                                       _tweetRating, 
                                       _linkThumbnailScreenFactory, 
                                       x => mockScreen, 
                                       null);
            test.Initialize();

            Assert.Same(mockScreen, test.InReplyToTweet);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsNotAReply_IsNull()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Null(test.InReplyToTweet);
        }

        [Fact]
        public void GettingLinks_WhenTweetContainsALink_ReturnsCollectionWithOpenedLinkScreen()
        {
            _linkThumbnailScreenFactory.CreateScreenForLink(Arg.Any<Uri>()).Returns(_linkThumbnailScreen);

            var test =
                new TweetScreen(A.Tweet.
                                    WithStatus("This tweet contains a link").
                                    LinkingTo(new Uri("http://link.one.com")).Build(), 
                                _tweetRating, 
                                _linkThumbnailScreenFactory, 
                                null, 
                                null);
            test.Initialize();

            Assert.Contains(_linkThumbnailScreen, test.Links);
        }

        [Fact]
        public void GettingMarkAsReadCommand__ReturnsCommand()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.IsType<MarkTweetAsReadCommand>(test.MarkAsReadCommand);
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsAMention_ReturnsVisible()
        {
            TweetScreen test = BuildDefaultTestSubject();
            _tweetRating.IsMention.Returns(true);

            Assert.Equal(Visibility.Visible, test.MentionVisibility);
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsNotAMention_ReturnsCollapsed()
        {
            TweetScreen test = BuildDefaultTestSubject();
            Assert.Equal(Visibility.Collapsed, test.MentionVisibility);
        }

        [Fact]
        public void GettingOpacity_WhenTweetIsNotRead_ReturnsOpaque()
        {
            TweetScreen test = BuildDefaultTestSubject();
            Assert.Equal(1.0, test.Opacity);
        }

        [Fact]
        public void GettingOpacity_WhenTweetIsRead_ReturnsSemitransparent()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Opacity).
                When(() => _tweet.MarkAsRead());
            Assert.Equal(.5, test.Opacity);
        }

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Status, test.Status);
        }

        [Fact]
        public void GettingUser__ReturnsUser()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.User, test.User);
        }

        private TweetScreen BuildDefaultTestSubject()
        {
            return new TweetScreen(_tweet, _tweetRating, _linkThumbnailScreenFactory, null, null);
        }
    }
}