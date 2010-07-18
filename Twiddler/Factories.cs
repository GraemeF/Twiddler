using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public class Factories
    {
        #region Delegates

        public delegate IImageThumbnailScreen ImageThumbnailScreenFactory(ImageLocations imageLocations);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(string id);

        public delegate ITweetScreen TweetScreenFactory(ITweet tweet);

        #endregion

        [Import]
        private CompositionContainer Container { get; set; }

        private TPart ComposePartWith<TPart, TImport>(TImport import)
        {
            var childContainer = new CompositionContainer(Container);
            childContainer.ComposeExportedValue(import);

            return childContainer.GetExportedValue<TPart>();
        }

        [Export(typeof (ImageThumbnailScreenFactory))]
        public IImageThumbnailScreen CreateImageThumbnailScreen(ImageLocations imageLocations)
        {
            return ComposePartWith<IImageThumbnailScreen, ImageLocations>(imageLocations);
        }

        [Export(typeof (LoadingTweetScreenFactory))]
        public ILoadingTweetScreen CreateLoadingTweetScreen(string id)
        {
            return ComposePartWith<ILoadingTweetScreen, string>(id);
        }

        [Export(typeof (TweetScreenFactory))]
        public ITweetScreen CreateTweetScreen(ITweet tweet)
        {
            return ComposePartWith<ITweetScreen, ITweet>(tweet);
        }
    }
}