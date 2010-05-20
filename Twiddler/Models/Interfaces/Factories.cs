using Twiddler.Screens.Interfaces;

namespace Twiddler.Models.Interfaces
{
    public static class Factories
    {
        public delegate ITweet Tweet(string status);
        public delegate ITweetScreen TweetScreen(ITweet tweet);
    }
}