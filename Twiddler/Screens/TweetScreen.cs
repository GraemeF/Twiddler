using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : Screen<TwitterStatus>, ITweetScreen
    {
        private readonly TwitterStatus _tweet;

        public TweetScreen(TwitterStatus tweet)
        {
            _tweet = tweet;
        }

        public string Status
        {
            get { return _tweet.Text; }
        }

        public TwitterUser User
        {
            get { return _tweet.User; }
        }

        public DateTime CreatedDate
        {
            get { return _tweet.CreatedDate; }
        }
    }
}