using System.Collections.Generic;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetStore : ITweetSink
    {
        Tweet GetTweet(string id);
        IEnumerable<Tweet> GetInboxTweets();
    }
}