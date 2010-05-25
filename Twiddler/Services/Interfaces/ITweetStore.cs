using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetStore
    {
        void AddTweet(Tweet tweet);
        Tweet GetTweet(TweetId id);
    }
}