using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Properties;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ICredentialsStore))]
    public class SettingsCredentialsStore : ICredentialsStore
    {
        #region ICredentialsStore Members

        public ITwitterCredentials Load()
        {
            return new TwitterCredentials(Settings.Default.AccessToken, Settings.Default.AccessTokenSecret);
        }

        public void Save(ITwitterCredentials credentials)
        {
            Settings.Default.AccessToken = credentials.Token;
            Settings.Default.AccessTokenSecret = credentials.TokenSecret;
            Settings.Default.Save();
        }

        #endregion
    }
}