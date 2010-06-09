﻿using System.Globalization;
using System.Linq;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler
{
    public static class Factories
    {
        #region Delegates

        public delegate IImageThumbnailScreen ImageThumbnailScreenFactory(ImageLocations imageLocations);

        public delegate ILoadingTweetScreen LoadingTweetScreenFactory(TweetId id);

        public delegate IFluentTwitter RequestFactory(ITwitterClient client);

        public delegate Tweet TweetFactory(TwitterStatus status);

        public delegate ITweetScreen TweetScreenFactory(Tweet tweet);

        public delegate User UserFactory(TwitterUser user);

        #endregion

        public static Tweet CreateTweetFromTwitterStatus(TwitterStatus status)
        {
            return new Tweet
                       {
                           Id = new TweetId(status.Id.ToString(CultureInfo.InvariantCulture)),
                           Status = status.Text,
                           User = CreateUserFromTwitterUser(status.User),
                           CreatedDate = status.CreatedDate,
                           Links = status.TextLinks.ToList()
                       };
        }

        private static User CreateUserFromTwitterUser(TwitterUser user)
        {
            return new User
                       {
                           Id = new UserId(user.Id.ToString(CultureInfo.InvariantCulture)),
                           Name = user.Name,
                           ProfileImageUrl = user.ProfileImageUrl,
                           ScreenName = user.ScreenName,
                           FollowersCount = user.FollowersCount,
                           IsVerified = user.IsVerified.GetValueOrDefault()
                       };
        }
    }
}