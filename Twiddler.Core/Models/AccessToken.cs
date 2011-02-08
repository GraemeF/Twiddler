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

        public string Id { get; private set; }

        public bool IsValid
        {
            get
            {
                return !(string.IsNullOrEmpty(Token) ||
                         string.IsNullOrEmpty(TokenSecret));
            }
        }

        public string Token { get; set; }

        public string TokenSecret { get; set; }
    }
}