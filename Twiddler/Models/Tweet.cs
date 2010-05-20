using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;

namespace Twiddler.Models
{
    [PerRequest(typeof (ITweet))]
    public class Tweet : ITweet
    {
        public Tweet(string status)
        {
            Status = status;
        }

        #region ITweet Members

        public string Status { get; private set; }

        #endregion
    }
}