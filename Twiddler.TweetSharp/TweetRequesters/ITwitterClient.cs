using TweetSharp;

namespace Twiddler.TweetSharp.TweetRequesters
{
    public interface ITwitterClient
    {
        TwitterService Service { get; }
    }
}