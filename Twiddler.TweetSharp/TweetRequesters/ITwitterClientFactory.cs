namespace Twiddler.TweetSharp.TweetRequesters
{
    #region Using Directives

    using global::TweetSharp;

    #endregion

    public interface ITwitterClientFactory
    {
        TwitterService CreateService();
    }
}