using System.Collections.Generic;
using Twiddler.Core.Models;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetRequester
    {
        IEnumerable<Tweet> RequestTweets();
    }
}