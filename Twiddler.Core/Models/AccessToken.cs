namespace Twiddler.Core.Models
{
    public class AccessToken
    {
        public const string DefaultCredentialsId = "DefaultCredentials";

        public AccessToken(string id,
                           string token,
                           string secret)
        {
            Id = id;
            TokenSecret = secret;
            Token = token;
        }

        #region IAccessToken Members

        public string Id { get; private set; }
        public string TokenSecret { get; set; }
        public string Token { get; set; }

        public bool IsValid
        {
            get
            {
                return !(string.IsNullOrEmpty(Token) ||
                         string.IsNullOrEmpty(TokenSecret));
            }
        }

        #endregion
    }
}