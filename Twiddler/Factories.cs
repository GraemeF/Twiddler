using System;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public static class Factories
    {
        #region Delegates

        public delegate ILinkScreen LinkScreenFactory(Uri uri);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(TweetId id);

        public delegate ITweetScreen TweetScreenFactory(TwitterStatus tweet);

        #endregion
    }
}