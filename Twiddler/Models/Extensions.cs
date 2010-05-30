using TweetSharp.Twitter.Model;

namespace Twiddler.Models
{
    public static class Extensions
    {
        public static TweetId GetTweetId(this TwitterStatus status)
        {
            return new TweetId(status.Id);
        }

        public static UserId GetUserId(this TwitterUser user)
        {
            return new UserId(user.Id);
        }
    }
}