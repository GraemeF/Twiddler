using System;
using System.Collections.Generic;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetStore : ITweetResolver
    {
        IEnumerable<ITweet> GetInboxTweets();
        event EventHandler<EventArgs> Updated;
    }
}