using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.TweetSharp.TweetRequesters;

namespace Twiddler.TweetSharp
{
    [Singleton(typeof (ITwitterClient))]
    [Export(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly ITwitterApplicationCredentials _applicationCredentials;

        [ImportingConstructor]
        public TwitterClient(IAccessTokenStore accessTokenStore, ITwitterApplicationCredentials applicationCredentials)
        {
            _accessTokenStore = accessTokenStore;
            _applicationCredentials = applicationCredentials;
        }

        #region ITwitterClient Members

        public TwitterService Service
        {
            get
            {
                AccessToken accessToken = _accessTokenStore.Load(AccessToken.DefaultCredentialsId);

                return new TwitterService(_applicationCredentials.ConsumerKey,
                                          _applicationCredentials.ConsumerSecret,
                                          accessToken.Token,
                                          accessToken.TokenSecret);
            }
        }

        #endregion
    }
}