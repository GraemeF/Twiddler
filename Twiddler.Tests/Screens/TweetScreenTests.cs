using Twiddler.Models;
using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class TweetScreenTests
    {
        private readonly Tweet _tweet = New.Tweet;

        [Fact]
        public void GettingStatus__ReturnsTweetStatus()
        {
            var test = new TweetScreen(_tweet);

            Assert.Equal(_tweet.Status, test.Status);
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

            Assert.Equal(_tweet.CreatedDate.ToString(), test.CreatedDate);
        }
    }
}