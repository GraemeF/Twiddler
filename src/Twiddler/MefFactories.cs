using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public class MefFactories
    {
        [Import]
        private CompositionContainer Container { get; set; }

        private TPart ComposePartWith<TPart, TImport>(TImport import)
        {
            var childContainer = new CompositionContainer(Container);
            childContainer.ComposeExportedValue(import);

            return childContainer.GetExportedValue<TPart>();
        }

        [Export(typeof (Factories.ImageThumbnailScreenFactory))]
        public IImageThumbnailScreen CreateImageThumbnailScreen(ImageLocations imageLocations)
        {
            return ComposePartWith<IImageThumbnailScreen, ImageLocations>(imageLocations);
        }

        [Export(typeof (Factories.LoadingTweetScreenFactory))]
        public ILoadingTweetScreen CreateLoadingTweetScreen(string id)
        {
            return ComposePartWith<ILoadingTweetScreen, string>(id);
        }

        [Export(typeof (Factories.TweetScreenFactory))]
        public ITweetScreen CreateTweetScreen(ITweet tweet)
        {
            return ComposePartWith<ITweetScreen, ITweet>(tweet);
        }
    }
}