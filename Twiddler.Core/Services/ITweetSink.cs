using TweetSharp.Twitter.Model;

namespace Twiddler.Core.Services
{
    public interface ITweetSink
    {
        /// <summary>
        /// Adds a tweet to the store.
        /// </summary>
        /// <param name="tweet">Tweet to add.</param>
        /// <returns>True if the tweet was added, false if it was already there.</returns>
        bool AddTweet(TwitterStatus tweet);
    }
}