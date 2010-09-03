namespace Twiddler.Core.Models
{
    public interface ITwitterApplicationCredentials
    {
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
    }
}