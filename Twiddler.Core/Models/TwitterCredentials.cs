namespace Twiddler.Core.Models
{
    public class TwitterCredentials : ITwitterCredentials
    {
        public const string DefaultCredentialsId = "DefaultCredentials";

        public TwitterCredentials(string id,
                                  string accessToken, string accessTokenSecret)
        {
            Id = id;
            ConsumerKey = "AQ8gY3dFm0R2FE4pjqGQ";
            ConsumerSecret = "nNN52t9DBk0QlwLcnlhY1z6b36LDRV1McGr9P243E";
            TokenSecret = accessTokenSecret;
            Token = accessToken;
        }

        #region ITwitterCredentials Members

        public string Id { get; private set; }
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string TokenSecret { get; set; }
        public string Token { get; set; }

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