using System.ComponentModel;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface IAuthorizer : INotifyPropertyChanged
    {
        User AuthenticatedUser { get; }
        AuthorizationStatus AuthorizationStatus { get; }
        void CheckAuthorization();
        void Deauthorize();
    }
}