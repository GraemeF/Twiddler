using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Linq;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
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

        public delegate Tweet TweetFactory(TwitterStatus status);

        public delegate ITweetRating TweetRatingFactory(Tweet tweet);

        public delegate ITweetScreen TweetScreenFactory(Tweet tweet);

        public delegate User UserFactory(TwitterUser user);

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
        public ITweetScreen CreateTweetScreen(Tweet tweet)
        {
            return ComposePartWith<ITweetScreen, Tweet>(tweet);
        }

        [Export(typeof (TweetRatingFactory))]
        public ITweetRating CreateTweetRating(Tweet tweet)
        {
            return ComposePartWith<ITweetRating, Tweet>(tweet);
        }

        [Export(typeof (TweetFactory))]
        public static Tweet CreateTweetFromTwitterStatus(TwitterStatus status)
        {
            return new Tweet
                       {
                           Id = status.Id.ToString(CultureInfo.InvariantCulture),
                           Status = status.Text,
                           User = CreateUserFromTwitterUser(status.User),
                           CreatedDate = status.CreatedDate,
                           Links = status.TextLinks.ToList(),
                           Mentions = status.TextMentions.ToList(),
                           InReplyToStatusId = GetInReplyToStatusId(status),
                           IsArchived = false,
                           IsRead = false
                       };
        }

        private static string GetInReplyToStatusId(TwitterStatus status)
        {
            return status.InReplyToStatusId.HasValue
                       ? status.InReplyToStatusId.ToString()
                       : null;
        }

        [Export(typeof (UserFactory))]
        public static User CreateUserFromTwitterUser(TwitterUser user)
        {
            return new User
                       {
                           Id = user.Id.ToString(CultureInfo.InvariantCulture),
                           Name = user.Name,
                           ProfileImageUrl = user.ProfileImageUrl,
                           ScreenName = user.ScreenName,
                           FollowersCount = user.FollowersCount,
                           IsVerified = user.IsVerified.GetValueOrDefault()
                       };
        }
    }
}