using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;

namespace Twiddler.Models
{
    [Singleton(typeof (ITwitterApplicationCredentials))]
    [Export(typeof (ITwitterApplicationCredentials))]
    public class TwiddlerApplication : ITwitterApplicationCredentials
    {
        #region ITwitterApplicationCredentials Members

        public string ConsumerKey
        {
            get { return "AQ8gY3dFm0R2FE4pjqGQ"; }
        }

        public string ConsumerSecret
        {
            get { return "nNN52t9DBk0QlwLcnlhY1z6b36LDRV1McGr9P243E"; }
        }

        #endregion
    }
}