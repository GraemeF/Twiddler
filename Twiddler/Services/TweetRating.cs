using System;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Models.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetRating))]
    public class TweetRating : ITweetRating
    {
        private readonly IUserInfo _userInfo;
        private readonly Tweet _tweet;

        public TweetRating(IUserInfo userInfo, Tweet tweet)
        {
            _userInfo = userInfo;
            _tweet = tweet;
        }

        #region ITweetRating Members

        public bool IsMention
        {
            get { return _tweet.Mentions.Contains(_userInfo.ScreenName); }
        }

        public bool IsDirectMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}