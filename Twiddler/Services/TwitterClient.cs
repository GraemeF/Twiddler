using System.ComponentModel;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ITwitterClient))]
    [Export(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
        private readonly ICredentialsStore _credentialsStore;
        private readonly Core.Factories.UserFactory _userFactory;
        private User _authenticatedUser;
        private AuthorizationStatus _authorizationStatus;
        private ITwitterCredentials _credentials;

        [ImportingConstructor]
        public TwitterClient(ICredentialsStore credentialsStore, Core.Factories.UserFactory userFactory)
        {
            _credentialsStore = credentialsStore;
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

        public IFluentTwitter MakeRequestFor()
        {
            return
                FluentTwitter.
                    CreateRequest().
                    AuthenticateWith(_credentials.ConsumerKey, _credentials.ConsumerSecret,
                                     _credentials.Token, _credentials.TokenSecret);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CheckAuthorization()
        {
            _credentials = _credentialsStore.Load();

            if (string.IsNullOrEmpty(_credentials.ConsumerKey) ||
                string.IsNullOrEmpty(_credentials.ConsumerSecret))
                AuthorizationStatus = AuthorizationStatus.InvalidApplication;
            else
                CheckCredentials();
        }

        public void Deauthorize()
        {
            AuthorizationStatus = AuthorizationStatus.NotAuthorized;

            _credentialsStore.Save(new TwitterCredentials(_credentials.ConsumerKey,
                                                          _credentials.ConsumerSecret,
                                                          null, null));
        }

        #endregion

        private void CheckCredentials()
        {
            if (_credentials.AreValid)
                VerifyCredentialsWithTwitter();
            else
                AuthorizationStatus = AuthorizationStatus.NotAuthorized;
        }

        private void VerifyCredentialsWithTwitter()
        {
            AuthorizationStatus = AuthorizationStatus.Verifying;

            ITwitterAccountVerifyCredentials twitter =
                FluentTwitter.
                    CreateRequest().
                    AuthenticateWith(_credentials.ConsumerKey, _credentials.ConsumerSecret,
                                     _credentials.Token, _credentials.TokenSecret).
                    Account().
                    VerifyCredentials();

            TwitterResult response = twitter.Request();
            TwitterUser profile = response.AsUser();

            AuthorizationStatus = profile != null
                                      ? AuthorizationStatus.Authorized
                                      : AuthorizationStatus.NotAuthorized;
            AuthenticatedUser = profile != null
                                    ? _userFactory(profile)
                                    : null;
        }
    }
}