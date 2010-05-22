using Twiddler.Models.Interfaces;

namespace Twiddler.Models
{
    public class TwitterCredentials : ITwitterCredentials
    {
        public TwitterCredentials(string consumerKey, string consumerSecret,
                                  string accessToken, string accessTokenSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            TokenSecret = accessTokenSecret;
            Token = accessToken;
        }

        #region ITwitterCredentials Members

        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string TokenSecret { get; private set; }
        public string Token { get; private set; }

        public bool AreValid
        {
            get
            {
                return !(string.IsNullOrEmpty(ConsumerKey) ||
                         string.IsNullOrEmpty(ConsumerSecret) ||
                         string.IsNullOrEmpty(Token) ||
                         string.IsNullOrEmpty(TokenSecret));
            }
        }

        #endregion
    }
}