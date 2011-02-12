namespace Twiddler.TweetSharp
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.Linq;

    using global::TweetSharp;

    using Twiddler.Core.Models;
    using Twiddler.TwitterStore.Models;

    #endregion

    public static class Factories
    {
        public delegate ITweet TweetFactory(TwitterStatus status);

        public delegate User UserFactory(TwitterUser user);

        [Export(typeof(TweetFactory))]
        public static Tweet CreateTweetFromTwitterStatus(TwitterStatus status)
        {
            return new Tweet
                       {
                           Id = status.Id.ToString(CultureInfo.InvariantCulture), 
                           Status = status.Text, 
                           User = CreateUserFromTwitterUser(status.User), 
                           CreatedDate = status.CreatedDate, 
                           Links = status.Entities.Urls.Select(x => new Uri(x.Value)).ToList(), 
                           Mentions = status.Entities.Mentions.Select(x => x.ScreenName).ToList(), 
                           InReplyToStatusId = GetInReplyToStatusId(status), 
                           IsArchived = false, 
                           IsRead = false
                       };
        }

        [Export(typeof(UserFactory))]
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

        private static string GetInReplyToStatusId(TwitterStatus status)
        {
            return status.InReplyToStatusId.HasValue
                       ? status.InReplyToStatusId.ToString()
                       : null;
        }
    }
}