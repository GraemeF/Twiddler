using TweetSharp.Twitter.Fluent;
using Twiddler.Models.Interfaces;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler
{
    public static class Factories
    {
        public delegate ITweet Tweet(string status);
        public delegate ITweetScreen TweetScreen(ITweet tweet);
        public delegate IFluentTwitter Request(ITwitterClient client);
    }
}