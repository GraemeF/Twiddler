namespace Twiddler.Core.Services
{
    public interface ITweetRating
    {
        bool IsMention { get; }
        bool IsDirectMessage { get; }
    }
}