using Moq;
using Twiddler.Core.Models;
using Twiddler.Services;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class TweetRatingTests
    {
        private const string TestScreenName = "MyScreenName";
        private readonly Mock<IUserInfo> _userInfo;
        private Tweet _tweet;

        public TweetRatingTests()
        {
            _userInfo = new Mock<IUserInfo>();
            _userInfo.Setup(x => x.ScreenName).Returns(TestScreenName);
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsNotMentioned_ReturnsFalse()
        {
            _tweet = A.Tweet;
            TweetRating test = BuildDefaultTestSubject();

            Assert.False(test.IsMention);
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsMentioned_ReturnsTrue()
        {
            _tweet = A.Tweet.Mentioning(TestScreenName);
            TweetRating test = BuildDefaultTestSubject();

            Assert.True(test.IsMention);
        }

        private TweetRating BuildDefaultTestSubject()
        {
            return new TweetRating(_userInfo.Object, _tweet);
        }
    }
}