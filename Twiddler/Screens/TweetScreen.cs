using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : Screen<Tweet>, ITweetScreen
    {
        private readonly Tweet _tweet;

        public TweetScreen(Tweet tweet)
        {
            _tweet = tweet;
        }

        public string Status
        {
            get { return _tweet.Status; }
        }

        public User User
        {
            get { return _tweet.User; }
        }

        public DateTime CreatedDate
        {
            get { return _tweet.CreatedDate; }
        }
    }
}