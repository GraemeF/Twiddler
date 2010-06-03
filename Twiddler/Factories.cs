using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public static class Factories
    {
        #region Delegates

        public delegate IImageThumbnailScreen ImageThumbnailScreenFactory(ImageLocations imageLocations);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(TweetId id);

        public delegate ITweetScreen TweetScreenFactory(TwitterStatus tweet);

        #endregion
    }
}