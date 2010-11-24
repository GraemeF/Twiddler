using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.TwitterStore.Models;

namespace Twiddler.TweetSharp
{
    public static class Factories
    {
        #region Delegates

        public delegate ITweet TweetFactory(TwitterStatus status);

        public delegate User UserFactory(TwitterUser user);

        #endregion

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