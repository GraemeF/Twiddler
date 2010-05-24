using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler
{
    public static class Factories
    {
        #region Delegates

        public delegate IFluentTwitter RequestFactory(ITwitterClient client);

        public delegate Tweet TweetFactory(TwitterStatus status);

        public delegate ITweetScreen TweetScreenFactory(Tweet tweet);

        public delegate User UserFactory(TwitterUser user);

        #endregion

        public static Tweet CreateTweetFromTwitterStatus(TwitterStatus status)
        {
            return new Tweet
                       {
                           Id = status.Id,
                           Status = status.Text,
                           User = CreateUserFromTwitterUser(status.User),
                           CreatedDate = status.CreatedDate
                       };
        }

        private static User CreateUserFromTwitterUser(TwitterUser user)
        {
            return new User
                       {
                           Id = user.Id,
                           Name = user.Name,
                           ProfileImageUrl = user.ProfileImageUrl,
                           ScreenName = user.ScreenName
                       };
        }
    }
}