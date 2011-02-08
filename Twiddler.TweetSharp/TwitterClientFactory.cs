namespace Twiddler.TweetSharp
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using global::TweetSharp;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TweetSharp.TweetRequesters;

    #endregion

    [Singleton(typeof(ITwitterClientFactory))]
    [Export(typeof(ITwitterClientFactory))]
    public class TwitterClientFactory : ITwitterClientFactory
    {
        private readonly IAccessTokenStore _accessTokenStore;

        private readonly ITwitterApplicationCredentials _applicationCredentials;

        [ImportingConstructor]
        public TwitterClientFactory(IAccessTokenStore accessTokenStore, 
                                    ITwitterApplicationCredentials applicationCredentials)
        {
            _accessTokenStore = accessTokenStore;
            _applicationCredentials = applicationCredentials;
        }

        #region ITwitterClientFactory members

        public TwitterService CreateService()
        {
            AccessToken accessToken = _accessTokenStore.Load(AccessToken.DefaultCredentialsId);

            return new TwitterService(_applicationCredentials.ConsumerKey, 
                                      _applicationCredentials.ConsumerSecret, 
                                      accessToken.Token, 
                                      accessToken.TokenSecret);
        }

        #endregion
    }
}