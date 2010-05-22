namespace Twiddler.Models.Interfaces
{
    public interface ITwitterCredentials
    {
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string TokenSecret { get; }
        string Token { get; }
        bool AreValid { get; }
    }
}