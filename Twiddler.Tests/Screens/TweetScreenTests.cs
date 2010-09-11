using System;
using System.Windows;
using Caliburn.Testability.Extensions;
using Moq;
using Twiddler.Commands;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TweetScreenTests
    {
        private readonly Mock<ILinkThumbnailScreenFactory> _fakeThumbnailFactory =
            new Mock<ILinkThumbnailScreenFactory>();

        private readonly Mock<ILinkThumbnailScreen> _fakeThumbnailScreen = new Mock<ILinkThumbnailScreen>();
        private readonly Mock<ITweetRating> _fakeTweetRating = new Mock<ITweetRating>();
        private readonly ITweet _tweet = A.Tweet.Build();

        [Fact]
        public void GettingMarkAsReadCommand__ReturnsCommand()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.IsType<MarkTweetAsReadCommand>(test.MarkAsReadCommand);
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Id, test.Id);
        }

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Status, test.Status);
        }

        private TweetScreen BuildDefaultTestSubject()
        {
            return new TweetScreen(_tweet, _fakeTweetRating.Object, _fakeThumbnailFactory.Object, null, null);
        }

        [Fact]
        public void GettingUser__ReturnsUser()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.User, test.User);
        }

        [Fact]
        public void GettingCreatedDate__ReturnsCreatedDate()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.CreatedDate, test.CreatedDate);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsNotAReply_IsNull()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Null(test.InReplyToTweet);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsAReply_IsALoadingTweetScreen()
        {
            var mockScreen = new Mock<ILoadingTweetScreen>();

            var test = new TweetScreen(A.Tweet.InReplyTo("4").Build(),
                                       _fakeTweetRating.Object,
                                       _fakeThumbnailFactory.Object,
                                       x => mockScreen.Object,
                                       null);
            test.Initialize();

            Assert.Same(mockScreen.Object, test.InReplyToTweet);
        }

        [Fact]
        public void GettingLinks_WhenTweetContainsALink_ReturnsCollectionWithOpenedLinkScreen()
        {
            _fakeThumbnailFactory.
                Setup(x => x.CreateScreenForLink(It.IsAny<Uri>())).
                Returns(_fakeThumbnailScreen.Object);

            var test =
                new TweetScreen(A.Tweet.
                                    WithStatus("This tweet contains a link").
                                    LinkingTo(new Uri("http://link.one.com")).Build(),
                                _fakeTweetRating.Object,
                                _fakeThumbnailFactory.Object,
                                null,
                                null);
            test.Initialize();

            Assert.Contains(_fakeThumbnailScreen.Object, test.Links);
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsNotAMention_ReturnsCollapsed()
        {
            TweetScreen test = BuildDefaultTestSubject();
            Assert.Equal(Visibility.Collapsed, test.MentionVisibility);
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsAMention_ReturnsVisible()
        {
            TweetScreen test = BuildDefaultTestSubject();
            _fakeTweetRating.
                Setup(x => x.IsMention).
                Returns(true);

            Assert.Equal(Visibility.Visible, test.MentionVisibility);
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
    }
}