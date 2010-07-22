using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public abstract class Factories
    {
        #region Delegates

        public delegate IImageThumbnailScreen ImageThumbnailScreenFactory(ImageLocations imageLocations);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(string id);

        public delegate ITweetScreen TweetScreenFactory(ITweet tweet);

        #endregion

        public void RegisterFactories()
        {
            RegisterFactory<ImageThumbnailScreenFactory>(CreateImageThumbnailScreen);
            RegisterFactory<LoadingTweetScreenFactory>(CreateLoadingTweetScreen);
            RegisterFactory<TweetScreenFactory>(CreateTweetScreen);
        }

        protected abstract void RegisterFactory<TFactory>(TFactory factory) where TFactory : class;

        protected abstract TPart ComposePartWith<TPart, TImport>(TImport import) where TImport : class;

        private IImageThumbnailScreen CreateImageThumbnailScreen(ImageLocations imageLocations)
        {
            return ComposePartWith<IImageThumbnailScreen, ImageLocations>(imageLocations);
        }

        private ILoadingTweetScreen CreateLoadingTweetScreen(string id)
        {
            return ComposePartWith<ILoadingTweetScreen, string>(id);
        }

        private ITweetScreen CreateTweetScreen(ITweet tweet)
        {
            return ComposePartWith<ITweetScreen, ITweet>(tweet);
        }
    }
}