using System.Collections.Generic;
using Twiddler.Core.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetRequester
    {
        IEnumerable<Tweet> RequestTweets();
    }
}