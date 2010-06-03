using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetStore : ITweetSink
    {
        TwitterStatus GetTweet(TweetId id);
    }
}