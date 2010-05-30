using Moq;
using TweetSharp.Twitter.Model;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TweetScreenTests
    {
        private readonly Mock<ILinkScreen> _fakeLinkScreen = new Mock<ILinkScreen>();
        private readonly TwitterStatus _tweet = New.Tweet;

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            TweetScreen test = BuildDefaultTestSubject();

            Assert.Equal(_tweet.Text, test.Status);
        }

        private TweetScreen BuildDefaultTestSubject()
        {
            return new TweetScreen(_tweet, x => _fakeLinkScreen.Object);
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
            var test =
                new TweetScreen(new TwitterStatus
                                    {Text = "This tweet contains a link to http://link.one.com"},
                                x => _fakeLinkScreen.Object);
            test.Initialize();

            _fakeLinkScreen.Verify(x => x.Initialize());
            Assert.Contains(_fakeLinkScreen.Object, test.Links);
        }
    }
}