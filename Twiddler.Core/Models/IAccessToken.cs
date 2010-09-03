namespace Twiddler.Core.Models
{
    public interface IAccessToken
    {
        string Id { get; }
        string TokenSecret { get; set; }
        string Token { get; set; }
        bool IsValid { get; }
    }
}