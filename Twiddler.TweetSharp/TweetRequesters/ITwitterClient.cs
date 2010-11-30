using TweetSharp.Twitter.Fluent;

namespace Twiddler.TweetSharp.TweetRequesters
{
    public interface ITwitterClient
    {
        IFluentTwitter MakeRequestFor();
    }
}