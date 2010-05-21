using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Models.Interfaces;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : Screen<ITweet>, ITweetScreen
    {
        private readonly ITweet _tweet;

        public TweetScreen(ITweet tweet)
        {
            _tweet = tweet;
        }

        public string Status
        {
            get { return _tweet.Status; }
        }
    }
}