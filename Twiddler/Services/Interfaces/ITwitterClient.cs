using System.ComponentModel;

namespace Twiddler.Services.Interfaces
{
    public interface ITwitterClient : INotifyPropertyChanged
    {
        AuthorizationStatus AuthorizationStatus { get; }
    }
}