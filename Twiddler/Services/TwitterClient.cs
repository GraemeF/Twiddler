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
        private readonly ITwitterCredentials _credentials;

        public TwitterClient(ITwitterCredentials credentials)
        {
            _credentials = credentials;
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

        private void PerformOAuthAuthorization()
        {
            var dlg = new OAuthDialog();
            bool? result = dlg.ShowDialog();

            if (result.HasValue == result.Value)
                VerifyCredentialsWithTwitter();
            else
                AuthorizationStatus = AuthorizationStatus.NotAuthorized;
        }

        private void CheckAuthorization()
        {
            if (string.IsNullOrEmpty(Settings.Default.ConsumerKey) ||
                string.IsNullOrEmpty(Settings.Default.ConsumerSecret))
                AuthorizationStatus = AuthorizationStatus.InvalidApplication;
            else
                CheckCredentials();
        }

        private void CheckCredentials()
        {
            if (string.IsNullOrEmpty(Settings.Default.AccessToken) ||
                string.IsNullOrEmpty(Settings.Default.AccessTokenSecret))
                AuthorizationStatus = AuthorizationStatus.NotAuthorized;
            else
                VerifyCredentialsWithTwitter();
        }

        private void VerifyCredentialsWithTwitter()
        {
            AuthorizationStatus = AuthorizationStatus.Verifying;

            ITwitterAccountVerifyCredentials twitter =
                FluentTwitter.
                    CreateRequest().
                    AuthenticateWith(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                     Settings.Default.AccessToken, Settings.Default.AccessTokenSecret).
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