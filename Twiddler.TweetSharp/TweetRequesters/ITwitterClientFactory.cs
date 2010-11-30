using TweetSharp;

namespace Twiddler.TweetSharp.TweetRequesters
{
    public interface ITwitterClientFactory
    {
        TwitterService CreateService();
    }
}