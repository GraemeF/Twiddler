namespace Twiddler.Models
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using Twiddler.Core.Models;

    #endregion

    [Singleton(typeof(ITwitterApplicationCredentials))]
    [Export(typeof(ITwitterApplicationCredentials))]
    public class TwiddlerApplication : ITwitterApplicationCredentials
    {
        public string ConsumerKey
        {
            get { return "AQ8gY3dFm0R2FE4pjqGQ"; }
        }

        public string ConsumerSecret
        {
            get { return "nNN52t9DBk0QlwLcnlhY1z6b36LDRV1McGr9P243E"; }
        }
    }
}