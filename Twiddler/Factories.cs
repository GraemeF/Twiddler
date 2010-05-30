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

        public delegate ITweetScreen TweetScreenFactory(TwitterStatus tweet);

        #endregion
    }
}