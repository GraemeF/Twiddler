using Twiddler.Models;

namespace Twiddler.Services
{
    public interface ITweetSink
    {
        void AddTweet(Tweet tweet);
    }
}