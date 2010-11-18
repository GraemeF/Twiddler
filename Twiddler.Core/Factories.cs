﻿using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Core
{
    public static class Factories
    {
        #region Delegates

        public delegate ITweet TweetFactory(IRawStatus status);

        public delegate User UserFactory(TwitterUser user);

        #endregion
    }
}