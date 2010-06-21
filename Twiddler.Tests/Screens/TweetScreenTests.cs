using System;
using System.Collections.Generic;
using Caliburn.Testability.Extensions;
using Moq;
using Twiddler.Commands.Interfaces;
using Twiddler.Core.Models;
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
        private readonly Tweet _tweet = New.Tweet;
        private Mock<IMarkTweetAsReadCommand> _fakeMarkAsReadCommand=new Mock<IMarkTweetAsReadCommand>();

        [Fact]
        public void GettingMarkAsReadCommand__ReturnsCommand()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Same(_fakeMarkAsReadCommand.Object,test.MarkAsReadCommand);

            Assert.True(_tweet.IsRead);
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
            return new TweetScreen(_tweet, _fakeThumbnailFactory.Object, null, _fakeMarkAsReadCommand.Object);
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

            var test = new TweetScreen(new Tweet
                                           {InReplyToStatusId = "4"},
                                       _fakeThumbnailFactory.Object,
                                       x => mockScreen.Object,
                                       _fakeMarkAsReadCommand.Object);
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
                new TweetScreen(
                    new Tweet
                        {
                            Status = "This tweet contains a link",
                            Links = new List<Uri> {new Uri("http://link.one.com"),}
                        },
                    _fakeThumbnailFactory.Object,
                    null,
                    _fakeMarkAsReadCommand.Object);
            test.Initialize();

            Assert.Contains(_fakeThumbnailScreen.Object, test.Links);
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