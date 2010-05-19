using Twiddler.Models.Interfaces;

namespace Twiddler.Models
{
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