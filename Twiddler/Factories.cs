namespace Twiddler
{
    #region Using Directives

    using Twiddler.Core.Models;
    using Twiddler.Models;
    using Twiddler.ViewModels.Interfaces;

    #endregion

    public abstract class Factories
    {
        public delegate IImageThumbnailScreen ImageThumbnailScreenFactory(ImageLocations imageLocations);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(string id);

        public delegate ITweetScreen TweetScreenFactory(ITweet tweet);

        public void RegisterFactories()
        {
            RegisterFactory<ImageThumbnailScreenFactory>(CreateImageThumbnailScreen);
            RegisterFactory<LoadingTweetScreenFactory>(CreateLoadingTweetScreen);
            RegisterFactory<TweetScreenFactory>(CreateTweetScreen);
        }

        protected abstract TPart ComposePartWith<TPart, TImport>(TImport import) where TImport : class;

        protected abstract void RegisterFactory<TFactory>(TFactory factory) where TFactory : class;

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