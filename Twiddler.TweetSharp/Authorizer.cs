namespace Twiddler.TweetSharp
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using ReactiveUI;

    using global::TweetSharp;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    [Singleton(typeof(IAuthorizer))]
    [Export(typeof(IAuthorizer))]
    public class Authorizer : ReactiveObject, 
                              IAuthorizer
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

        public User AuthenticatedUser
        {
            get { return _authenticatedUser; }
            private set { this.RaiseAndSetIfChanged(x => x.AuthenticatedUser, value); }
        }

        public AuthorizationStatus AuthorizationStatus
        {
            get { return _authorizationStatus; }
            private set { this.RaiseAndSetIfChanged(x => x.AuthorizationStatus, value); }
        }

        #region IAuthorizer members

        public void CheckAuthorization()
        {
            _accessToken = _accessTokenStore.Load(AccessToken.DefaultCredentialsId);

            CheckCredentials();
        }

        public void Deauthorize()
        {
            AuthorizationStatus = AuthorizationStatus.Unauthorized;

            _accessTokenStore.Save(new AccessToken(AccessToken.DefaultCredentialsId, 
                                                   null, 
                                                   null));
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