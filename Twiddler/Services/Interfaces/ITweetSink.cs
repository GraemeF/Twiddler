using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSink
    {
        /// <summary>
        /// Adds a tweet to the store.
        /// </summary>
        /// <param name="tweet">Tweet to add.</param>
        /// <returns>True if the tweet was added, false if it was already there.</returns>
        bool AddTweet(Tweet tweet);
    }
}