using System;
using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;
using Twiddler.Properties;

namespace Twiddler.Models
{
    [Singleton(typeof (ITwitterCredentials))]
    public class TwitterCredentials : ITwitterCredentials
    {
        public TwitterCredentials()
        {
            Settings.Default.SettingsSaving += (sender, args) => CredentialsChanged(this, EventArgs.Empty);
        }

        #region ITwitterCredentials Members

        public string TokenSecret
        {
            get { return Settings.Default.AccessTokenSecret; }
        }

        public string Token
        {
            get { return Settings.Default.AccessToken; }
        }

        public event EventHandler<EventArgs> CredentialsChanged;

        #endregion
    }
}