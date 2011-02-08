namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System.Collections.Generic;

    using Twiddler.Core.Models;

    #endregion

    public interface ITweetRequester
    {
        IEnumerable<ITweet> RequestTweets();
    }
}