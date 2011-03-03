namespace Twiddler.Tests.Services
{
    #region Using Directives

    using NSubstitute;

    using ReactiveUI.Testing;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class TweetRatingTests
    {
        private readonly IAuthorizer _authorizer = Substitute.For<IAuthorizer>().WithReactiveProperties();

        private readonly User _user = A.User;

        private ITweet _tweet = A.Tweet.Build();

        public TweetRatingTests()
        {
            _authorizer.AuthenticatedUser.Returns(_user);
        }

        [Fact]
        public void GettingIsMention_WhenTheUserBecomesAuthenticatedAndIsMentioned_ReturnsTrue()
        {
            _tweet = A.Tweet.Mentioning(_user.ScreenName).Build();
            ThereIsNoAuthenticatedUser();
            TweetRating test = BuildDefaultTestSubject();

            test.AssertThatChangeNotificationIsRaisedBy(x => x.IsMention, 
                                                        () => _authorizer.
                                                                  PropertyChanges(x => x.AuthenticatedUser, 
                                                                                  _user));

            test.IsMention.Should().Be.True();
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsMentioned_ReturnsTrue()
        {
            _tweet = A.Tweet.Mentioning(_user.ScreenName).Build();
            TweetRating test = BuildDefaultTestSubject();

            test.IsMention.Should().Be.True();
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsNotAuthenticated_ReturnsFalse()
        {
            ThereIsNoAuthenticatedUser();

            TweetRating test = BuildDefaultTestSubject();

            test.IsMention.Should().Be.False();
        }

        [Fact]
        public void GettingIsMention_WhenTheUserIsNotMentioned_ReturnsFalse()
        {
            TweetRating test = BuildDefaultTestSubject();

            test.IsMention.Should().Be.False();
        }

        [Fact]
        public void GettingIsRead_WhenTheTweetBecomesRead_ReturnsTrue()
        {
            _tweet = A.Tweet.WhichIsUnread().Build();
            TweetRating test = BuildDefaultTestSubject();

            test.AssertThatChangeNotificationIsRaisedBy(x => x.IsRead, 
                                                        () => _tweet.PropertyChanges(x => x.IsRead, true));

            test.IsRead.Should().Be.True();
        }

        [Fact]
        public void GettingIsRead_WhenTheTweetHasBeenRead_ReturnsTrue()
        {
            _tweet = A.Tweet.WhichHasBeenRead().Build();
            TweetRating test = BuildDefaultTestSubject();

            test.IsRead.Should().Be.True();
        }

        private TweetRating BuildDefaultTestSubject()
        {
            return new TweetRating(_authorizer, _tweet);
        }

        private void ThereIsNoAuthenticatedUser()
        {
            _authorizer.AuthenticatedUser.Returns((User)null);
        }
    }
}