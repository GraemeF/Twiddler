using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.TwitterStore.Models;

namespace Twiddler.TwitterStore
{
    internal class Factories
    {
        [Export(typeof (Core.Factories.TweetFactory))]
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

        [Export(typeof (Core.Factories.UserFactory))]
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