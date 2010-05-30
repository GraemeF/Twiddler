using TweetSharp.Twitter.Model;
using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TweetScreenTests
    {
        private readonly TwitterStatus _tweet = New.Tweet;

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            var test = new TweetScreen(_tweet);

            Assert.Equal(_tweet.Text, test.Status);
        }

        [Fact]
        public void GettingUser__ReturnsUser()
        {
            var test = new TweetScreen(_tweet);

            Assert.Equal(_tweet.User, test.User);
        }

        [Fact]
        public void GettingCreatedDate__ReturnsCreatedDate()
        {
            var test = new TweetScreen(_tweet);

            Assert.Equal(_tweet.CreatedDate, test.CreatedDate);
        }
    }
}