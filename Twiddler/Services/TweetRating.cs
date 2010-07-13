using System;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetRating))]
    public class TweetRating : ITweetRating
    {
        private readonly ITwitterClient _client;
        private readonly Tweet _tweet;

        public TweetRating(ITwitterClient client, Tweet tweet)
        {
            _client = client;
            _tweet = tweet;
        }

        #region ITweetRating Members

        public bool IsMention
        {
            get
            {
                User authenticatedUser = _client.AuthenticatedUser;
                return authenticatedUser != null
                       && _tweet.Mentions.Contains(authenticatedUser.ScreenName);
            }
        }

        public bool IsDirectMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}