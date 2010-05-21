using Twiddler.Models.Interfaces;

namespace Twiddler.Models
{
    public class TwitterCredentials : ITwitterCredentials
    {
        public TwitterCredentials(string accessToken, string accessTokenSecret)
        {
            TokenSecret = accessTokenSecret;
            Token = accessToken;
        }

        #region ITwitterCredentials Members

        public string TokenSecret { get; private set; }
        public string Token { get; private set; }

        #endregion
    }
}