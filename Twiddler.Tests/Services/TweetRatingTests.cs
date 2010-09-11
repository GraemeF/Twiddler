using Moq;
using Twiddler.Core.Models;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class TweetRatingTests
    {
        private ITweet _tweet;
        private User _user = A.User;

        [Fact]
        public void GettingIsMention_WhenTheUserIsNotAuthenticated_ReturnsFalse()
        {
            _tweet = A.Tweet.Build();
            _user = null;
            TweetRating test = BuildDefaultTestSubject();

            Assert.False(test.IsMention);
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsNotMentioned_ReturnsFalse()
        {
            _tweet = A.Tweet.Build();
            TweetRating test = BuildDefaultTestSubject();

            Assert.False(test.IsMention);
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsMentioned_ReturnsTrue()
        {
            _tweet = A.Tweet.Mentioning(_user.ScreenName).Build();
            TweetRating test = BuildDefaultTestSubject();

            Assert.True(test.IsMention);
        }

        private TweetRating BuildDefaultTestSubject()
        {
            var fakeClient = new Mock<ITwitterClient>();
            fakeClient.
                Setup(x => x.AuthenticatedUser).
                Returns(_user);

            return new TweetRating(fakeClient.Object, _tweet);
        }
    }
}