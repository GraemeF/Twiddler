namespace Twiddler.Core.Services
{
    #region Using Directives

    using System.Collections.Generic;

    using Twiddler.Core.Models;

    #endregion

    public interface ITweetSink
    {
        /// <summary>
        /// 	Adds a tweet to the store.
        /// </summary>
        /// <param name="tweet">
        /// Tweet to add.
        /// </param>
        void Add(ITweet tweet);

        void Add(IEnumerable<ITweet> tweets);
    }
}