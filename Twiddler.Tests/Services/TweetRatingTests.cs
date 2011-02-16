namespace Twiddler.Tests.Services
{
    #region Using Directives

    using NSubstitute;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class TweetRatingTests
    {
        private ITweet _tweet;

        private User _user = A.User;

        [Fact]
        public void GettingIsMention_WhenTheUserIsMentioned_ReturnsTrue()
        {
            _tweet = A.Tweet.Mentioning(_user.ScreenName).Build();
            TweetRating test = BuildDefaultTestSubject();

            Assert.True(test.IsMention);
        }

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

        private TweetRating BuildDefaultTestSubject()
        {
            var authorizer = Substitute.For<IAuthorizer>();
            authorizer.AuthenticatedUser.Returns(_user);

            return new TweetRating(authorizer, _tweet);
        }
    }
}