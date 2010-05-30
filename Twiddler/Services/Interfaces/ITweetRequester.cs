using System.Collections.Generic;
using TweetSharp.Twitter.Model;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetRequester
    {
        IEnumerable<TwitterStatus> RequestTweets();
    }
}