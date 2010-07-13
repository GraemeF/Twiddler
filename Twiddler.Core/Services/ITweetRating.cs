using System.ComponentModel;

namespace Twiddler.Core.Services
{
    public interface ITweetRating : INotifyPropertyChanged
    {
        bool IsMention { get; }
        bool IsDirectMessage { get; }
    }
}