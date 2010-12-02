using System.Collections.Generic;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetStore : ITweetResolver, ITweetSink
    {
        IEnumerable<ITweet> GetInboxTweets();
    }
}