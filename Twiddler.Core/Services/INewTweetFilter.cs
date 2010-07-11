using System.Collections.Generic;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface INewTweetFilter
    {
        IEnumerable<Tweet> RemoveKnownTweets(IEnumerable<Tweet> tweets);
    }
}