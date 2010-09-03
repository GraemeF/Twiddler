namespace Twiddler.Core.Models
{
    public interface ITwitterCredentials
    {
        string Id { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string TokenSecret { get; }
        string Token { get; }
        bool AreValid { get; }
    }
}