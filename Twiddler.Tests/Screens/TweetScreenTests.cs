using System;
using Moq;
using TweetSharp.Twitter.Model;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TweetScreenTests
    {
        private readonly Mock<ILinkThumbnailScreenFactory> _fakeThumbnailFactory =
            new Mock<ILinkThumbnailScreenFactory>();

        private readonly Mock<ILinkThumbnailScreen> _fakeThumbnailScreen = new Mock<ILinkThumbnailScreen>();
        private readonly TwitterStatus _tweet = New.Tweet;

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Text, test.Status);
        }

        private TweetScreen BuildDefaultTestSubject()
        {
            return new TweetScreen(_tweet, _fakeThumbnailFactory.Object);
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
        public void GettingLinks_WhenTweetContainsALink_ReturnsCollectionWithOpenedLinkScreen()
        {
            _fakeThumbnailFactory.
                Setup(x => x.CreateScreenForLink(It.IsAny<Uri>())).
                Returns(_fakeThumbnailScreen.Object);

            var test =
                new TweetScreen(new TwitterStatus
                                    {Text = "This tweet contains a link to http://link.one.com"},
                                _fakeThumbnailFactory.Object);
            test.Initialize();

            Assert.Contains(_fakeThumbnailScreen.Object, test.Links);
        }
    }
}