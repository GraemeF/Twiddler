using System.ComponentModel;

namespace Twiddler.Services.Interfaces
{
    public interface IRequestStatus : INotifyPropertyChanged
    {
        int HourlyLimit { get; }
        int RemainingHits { get; }
    }
}