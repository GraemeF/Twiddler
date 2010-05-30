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

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(TweetId id);

        public delegate IFluentTwitter RequestFactory(ITwitterClient client);

        public delegate TwitterStatus TweetFactory(TwitterStatus status);

        public delegate ITweetScreen TweetScreenFactory(TwitterStatus tweet);

        public delegate TwitterUser UserFactory(TwitterUser user);

        #endregion

        public static TwitterStatus CreateTweetFromTwitterStatus(TwitterStatus status)
        {
            return status;
        }

        private static TwitterUser CreateUserFromTwitterUser(TwitterUser user)
        {
            return user;
        }
    }
}