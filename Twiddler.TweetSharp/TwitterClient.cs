using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.TweetSharp
{
    [Singleton(typeof (ITwitterClient))]
    [Export(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly ITwitterApplicationCredentials _applicationCredentials;
        private readonly Factories.UserFactory _userFactory;
        private AccessToken _accessToken;
        private User _authenticatedUser;
        private AuthorizationStatus _authorizationStatus;

        [ImportingConstructor]
        public TwitterClient(ITwitterApplicationCredentials applicationCredentials,
                             IAccessTokenStore accessTokenStore,
                             Factories.UserFactory userFactory)
        {
            _applicationCredentials = applicationCredentials;
            _accessTokenStore = accessTokenStore;
            _userFactory = userFactory;
        }

        #region ITwitterClient Members

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

        public ITwitterRequest CreateRequestForStatusesOnFriendsTimeline(long since)
        {
            return new TweetSharpRequest(FluentTwitter.CreateRequest().
                                             AuthenticateWith(_applicationCredentials.ConsumerKey,
                                                              _applicationCredentials.ConsumerSecret,
                                                              _accessToken.Token,
                                                              _accessToken.TokenSecret));
        }

        public ITwitterRequest CreateRequestForStatusesOnHomeTimeline(long since)
        {
            throw new NotImplementedException();
        }

        public ITwitterRequest CreateRequestForMentions(long since)
        {
            throw new NotImplementedException();
        }

        public ITwitterRequest CreateRequestForRetweetsOfMe(long since)
        {
            throw new NotImplementedException();
        }

        public ITwitterRequest CreateRequestForStatusesOnUserTimeline(long since)
        {
            throw new NotImplementedException();
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

            ITwitterAccountVerifyCredentials twitter =
                FluentTwitter.
                    CreateRequest().
                    AuthenticateWith(_applicationCredentials.ConsumerKey, _applicationCredentials.ConsumerSecret,
                                     _accessToken.Token, _accessToken.TokenSecret).
                    Account().
                    VerifyCredentials();

            TwitterResult response = twitter.Request();
            TwitterUser profile = response.AsUser();

            AuthorizationStatus = profile != null
                                      ? AuthorizationStatus.Authorized
                                      : AuthorizationStatus.Unauthorized;
            AuthenticatedUser = profile != null
                                    ? _userFactory(profile)
                                    : null;
        }
    }
}