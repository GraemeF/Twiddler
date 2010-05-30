using TweetSharp.Twitter.Model;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetStore : ITweetSink
    {
        TwitterStatus GetTweet(TweetId id);
    }
}