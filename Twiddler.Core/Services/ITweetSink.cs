using System.Collections.Generic;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetSink
    {
        /// <summary>
        /// Adds a tweet to the store.
        /// </summary>
        /// <param name="tweet">Tweet to add.</param>
        void Add(ITweet tweet);

        void Add(IEnumerable<ITweet> tweets);
    }
}