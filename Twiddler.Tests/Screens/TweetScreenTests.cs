namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;
    using System.Windows;

    using Caliburn.Testability.Extensions;

    using NSubstitute;

    using Should.Fluent;

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
        private readonly ILinkThumbnailScreen _linkThumbnailScreen = Substitute.For<ILinkThumbnailScreen>();

        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory =
            Substitute.For<ILinkThumbnailScreenFactory>();

        private readonly ITweet _tweet = A.Tweet.Build();

        private readonly ITweetRating _tweetRating = Substitute.For<ITweetRating>();

        [Fact]
        public void GettingCreatedDate__ReturnsCreatedDate()
        {
            TweetScreen test = BuildDefaultTestSubject();

            test.CreatedDate.Should().Equal(_tweet.CreatedDate);
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            TweetScreen test = BuildDefaultTestSubject();

            test.Id.Should().Equal(_tweet.Id);
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

            test.InReplyToTweet.Should().Be.SameAs(mockScreen);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsNotAReply_IsNull()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.InReplyToTweet.Should().Be.Null();
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

            test.Links.Should().Contain.Item(_linkThumbnailScreen);
        }

        [Fact]
        public void GettingMarkAsReadCommand__ReturnsCommand()
        {
            TweetScreen test = BuildDefaultTestSubject();

            test.MarkAsReadCommand.Should().Be.OfType<MarkTweetAsReadCommand>();
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsAMention_ReturnsVisible()
        {
            TweetScreen test = BuildDefaultTestSubject();
            _tweetRating.IsMention.Returns(true);

            test.MentionVisibility.Should().Equal(Visibility.Visible);
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsNotAMention_ReturnsCollapsed()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.MentionVisibility.Should().Equal(Visibility.Collapsed);
        }

        [Fact]
        public void GettingOpacity_WhenTweetIsNotRead_ReturnsOpaque()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.Opacity.Should().Equal(1.0);
        }

        [Fact]
        public void GettingOpacity_WhenTweetIsRead_ReturnsSemitransparent()
        {
            TweetScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Opacity).
                When(() => _tweet.MarkAsRead());
            test.Opacity.Should().Equal(0.5);
        }

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            TweetScreen test = BuildDefaultTestSubject();

            test.Status.Should().Equal(_tweet.Status);
        }

        [Fact]
        public void GettingUser__ReturnsUser()
        {
            TweetScreen test = BuildDefaultTestSubject();

            test.User.Should().Be.SameAs(_tweet.User);
        }

        private TweetScreen BuildDefaultTestSubject()
        {
            return new TweetScreen(_tweet, _tweetRating, _linkThumbnailScreenFactory, null, null);
        }
    }
}