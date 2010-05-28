using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSink
    {
        void AddTweet(Tweet tweet);
    }
}