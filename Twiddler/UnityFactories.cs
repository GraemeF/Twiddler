using Microsoft.Practices.Unity;
using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public class UnityFactories
    {
        private readonly IUnityContainer _container;

        public UnityFactories(IUnityContainer container)
        {
            _container = container;
        }

        public void RegisterFactories()
        {
            _container.RegisterInstance<Factories.ImageThumbnailScreenFactory>(CreateImageThumbnailScreen);
            _container.RegisterInstance<Factories.LoadingTweetScreenFactory>(CreateLoadingTweetScreen);
            _container.RegisterInstance<Factories.TweetScreenFactory>(CreateTweetScreen);
        }

        private TPart ComposePartWith<TPart, TImport>(TImport import)
        {
            IUnityContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance(import);

            return childContainer.Resolve<TPart>();
        }

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