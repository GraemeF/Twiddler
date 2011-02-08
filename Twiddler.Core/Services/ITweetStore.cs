namespace Twiddler.Core.Services
{
    #region Using Directives

    using System.Collections.Generic;

    using Twiddler.Core.Models;

    #endregion

    public interface ITweetStore : ITweetResolver, 
                                   ITweetSink
    {
        IEnumerable<ITweet> GetInboxTweets();
    }
}