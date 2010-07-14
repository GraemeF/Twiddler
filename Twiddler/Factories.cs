using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TweetSharp.Twitter.Fluent;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler
{
    public class Factories
    {
        #region Delegates

        public delegate IImageThumbnailScreen ImageThumbnailScreenFactory(ImageLocations imageLocations);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(string id);

        public delegate IFluentTwitter RequestFactory(ITwitterClient client);

        public delegate ITweetRating TweetRatingFactory(ITweet tweet);

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

        [Export(typeof (TweetRatingFactory))]
        public ITweetRating CreateTweetRating(ITweet tweet)
        {
            return ComposePartWith<ITweetRating, ITweet>(tweet);
        }
    }
}