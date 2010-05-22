using System.ComponentModel;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Models.Interfaces;
using Twiddler.Properties;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
        private readonly ICredentialsStore _credentialsStore;
        private ITwitterCredentials _credentials;

        public TwitterClient(ICredentialsStore credentialsStore)
        {
            _credentialsStore = credentialsStore;
        }

        #region ITwitterClient Members

        public AuthorizationStatus AuthorizationStatus { get; private set; }

        public IFluentTwitter MakeRequestFor()
        {
            return
                FluentTwitter.
                    CreateRequest().
                    AuthenticateWith(_credentials.Token, _credentials.TokenSecret);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void CheckAuthorization()
        {
            _credentials = _credentialsStore.Load();

            if (string.IsNullOrEmpty(Settings.Default.ConsumerKey) ||
                string.IsNullOrEmpty(Settings.Default.ConsumerSecret))
                AuthorizationStatus = AuthorizationStatus.InvalidApplication;
            else
                CheckCredentials();
        }

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
                    AuthenticateWith(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                     _credentials.Token, _credentials.TokenSecret).
                    Account().
                    VerifyCredentials();

            TwitterResult response = twitter.Request();
            TwitterUser profile = response.AsUser();

            AuthorizationStatus = profile != null
                                      ? AuthorizationStatus.Authorized
                                      : AuthorizationStatus.NotAuthorized;
        }
    }
}