namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using System;
    using System.Windows;

    using Caliburn.Micro;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Commands;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.ViewModels;
    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class TweetViewModelTests
    {
        private readonly ILinkThumbnailScreen _linkThumbnailScreen = Substitute.For<ILinkThumbnailScreen>();

        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory =
            Substitute.For<ILinkThumbnailScreenFactory>();

        private readonly ITweet _tweet = A.Tweet.Build();

        private readonly ITweetRating _tweetRating = Substitute.For<ITweetRating>();

        [Fact]
        public void GettingCreatedDate__ReturnsCreatedDate()
        {
            TweetViewModel test = BuildDefaultTestSubject();

            test.CreatedDate.Should().Equal(_tweet.CreatedDate);
        }

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            TweetViewModel test = BuildDefaultTestSubject();

            test.Id.Should().Equal(_tweet.Id);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsAReply_IsALoadingTweetScreen()
        {
            var mockScreen = Substitute.For<ILoadingTweetScreen>();

            var test = new TweetViewModel(A.Tweet.InReplyTo("4").Build(), 
                                       _tweetRating, 
                                       _linkThumbnailScreenFactory, 
                                       x => mockScreen, 
                                       null);
            ((IActivate)test).Activate();

            test.InReplyToTweet.Should().Be.SameAs(mockScreen);
        }

        [Fact]
        public void GettingInReplyToTweet_WhenTheTweetIsNotAReply_IsNull()
        {
            TweetViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.InReplyToTweet.Should().Be.Null();
        }

        [Fact]
        public void GettingLinks_WhenTweetContainsALink_ReturnsCollectionWithOpenedLinkScreen()
        {
            _linkThumbnailScreenFactory.CreateScreenForLink(Arg.Any<Uri>()).Returns(_linkThumbnailScreen);

            var test =
                new TweetViewModel(A.Tweet.
                                    WithStatus("This tweet contains a link").
                                    LinkingTo(new Uri("http://link.one.com")).Build(), 
                                _tweetRating, 
                                _linkThumbnailScreenFactory, 
                                null, 
                                null);
            ((IActivate)test).Activate();

            test.Links.Should().Contain.Item(_linkThumbnailScreen);
        }

        [Fact]
        public void GettingMarkAsReadCommand__ReturnsCommand()
        {
            TweetViewModel test = BuildDefaultTestSubject();

            test.MarkAsReadCommand.Should().Be.OfType<MarkTweetAsReadCommand>();
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsAMention_ReturnsVisible()
        {
            TweetViewModel test = BuildDefaultTestSubject();
            _tweetRating.IsMention.Returns(true);

            test.MentionVisibility.Should().Equal(Visibility.Visible);
        }

        [Fact]
        public void GettingMentionVisibility_WhenTweetIsNotAMention_ReturnsCollapsed()
        {
            TweetViewModel test = BuildDefaultTestSubject();
            test.MentionVisibility.Should().Equal(Visibility.Collapsed);
        }

        [Fact]
        public void GettingOpacity_WhenTweetIsNotRead_ReturnsOpaque()
        {
            TweetViewModel test = BuildDefaultTestSubject();
            test.Opacity.Should().Equal(1.0);
        }

        [Fact]
        public void GettingOpacity_WhenTweetIsRead_ReturnsSemitransparent()
        {
            TweetViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Opacity, () => _tweet.MarkAsRead());
            test.Opacity.Should().Equal(0.5);
        }

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            TweetViewModel test = BuildDefaultTestSubject();

            test.Status.Should().Equal(_tweet.Status);
        }

        [Fact]
        public void GettingUser__ReturnsUser()
        {
            TweetViewModel test = BuildDefaultTestSubject();

            test.User.Should().Be.SameAs(_tweet.User);
        }

        private TweetViewModel BuildDefaultTestSubject()
        {
            return new TweetViewModel(_tweet, _tweetRating, _linkThumbnailScreenFactory, null, null);
        }
    }
}