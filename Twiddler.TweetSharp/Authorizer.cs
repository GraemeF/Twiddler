using System.ComponentModel;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.TweetSharp
{
    [Singleton(typeof (IAuthorizer))]
    [Export(typeof (IAuthorizer))]
    public class Authorizer : IAuthorizer
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly ITwitterApplicationCredentials _applicationCredentials;
        private readonly Factories.UserFactory _userFactory;
        private AccessToken _accessToken;
        private User _authenticatedUser;
        private AuthorizationStatus _authorizationStatus;

        [ImportingConstructor]
        public Authorizer(ITwitterApplicationCredentials applicationCredentials,
                          IAccessTokenStore accessTokenStore,
                          Factories.UserFactory userFactory)
        {
            _applicationCredentials = applicationCredentials;
            _accessTokenStore = accessTokenStore;
            _userFactory = userFactory;
        }

        #region IAuthorizer Members

        public User AuthenticatedUser
        {
            get { return _authenticatedUser; }
            private set
            {
                if (_authenticatedUser != value)
                {
                    _authenticatedUser = value;
                    PropertyChanged.Raise(x => AuthenticatedUser);
                }
            }
        }

        public AuthorizationStatus AuthorizationStatus
        {
            get { return _authorizationStatus; }
            private set
            {
                if (_authorizationStatus != value)
                {
                    _authorizationStatus = value;
                    PropertyChanged.Raise(x => AuthorizationStatus);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CheckAuthorization()
        {
            _accessToken = _accessTokenStore.Load(AccessToken.DefaultCredentialsId);

            CheckCredentials();
        }

        public void Deauthorize()
        {
            AuthorizationStatus = AuthorizationStatus.Unauthorized;

            _accessTokenStore.Save(new AccessToken(AccessToken.DefaultCredentialsId,
                                                   null, null));
        }

        #endregion

        private void CheckCredentials()
        {
            if (_accessToken.IsValid)
                VerifyCredentialsWithTwitter();
            else
                AuthorizationStatus = AuthorizationStatus.Unauthorized;
        }

        private void VerifyCredentialsWithTwitter()
        {
            AuthorizationStatus = AuthorizationStatus.Verifying;

            var service = new TwitterService(_applicationCredentials.ConsumerKey,
                                             _applicationCredentials.ConsumerSecret,
                                             _accessToken.Token,
                                             _accessToken.TokenSecret);

            TwitterUser profile = service.VerifyCredentials();

            AuthorizationStatus = profile != null
                                      ? AuthorizationStatus.Authorized
                                      : AuthorizationStatus.Unauthorized;
            AuthenticatedUser = profile != null
                                    ? _userFactory(profile)
                                    : null;
        }
    }
}