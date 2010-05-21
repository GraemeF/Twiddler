using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;
using Twiddler.Properties;

namespace Twiddler.Models
{
    [Singleton(typeof (ITwitterCredentials))]
    public class TwitterCredentials : ITwitterCredentials
    {
        #region ITwitterCredentials Members

        public string TokenSecret
        {
            get { return Settings.Default.AccessTokenSecret; }
        }

        public string Token
        {
            get { return Settings.Default.AccessToken; }
        }

        #endregion
    }
}