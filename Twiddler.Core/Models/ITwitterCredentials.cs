namespace Twiddler.Core.Models
{
    public interface ITwitterCredentials
    {
        string Id { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string TokenSecret { get; set; }
        string Token { get; set; }
        bool AreValid { get; }
    }
}