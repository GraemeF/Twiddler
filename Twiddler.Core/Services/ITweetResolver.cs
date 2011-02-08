namespace Twiddler.Core.Services
{
    #region Using Directives

    using Twiddler.Core.Models;

    #endregion

    public interface ITweetResolver
    {
        ITweet GetTweet(string id);
    }
}