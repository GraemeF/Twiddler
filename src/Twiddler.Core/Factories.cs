using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;

namespace Twiddler.Core
{
    public static class Factories
    {
        #region Delegates

        public delegate ITweet TweetFactory(TwitterStatus status);

        public delegate User UserFactory(TwitterUser user);

        #endregion
    }
}