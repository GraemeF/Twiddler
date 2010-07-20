using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Properties;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ICredentialsStore))]
    [Export(typeof (ICredentialsStore))]
    [NoCoverage]
    public class SettingsCredentialsStore : ICredentialsStore
    {
        #region ICredentialsStore Members

        public ITwitterCredentials Load()
        {
            return new TwitterCredentials(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                          Settings.Default.AccessToken, Settings.Default.AccessTokenSecret);
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