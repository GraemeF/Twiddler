using Twiddler.Models.Interfaces;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public static class Factories
    {
        public delegate ITweet Tweet(string status);
        public delegate ITweetScreen TweetScreen(ITweet tweet);
    }
}