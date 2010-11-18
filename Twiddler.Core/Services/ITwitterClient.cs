using System.ComponentModel;
using Twiddler.Core.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITwitterClient : INotifyPropertyChanged
    {
        User AuthenticatedUser { get; }
        AuthorizationStatus AuthorizationStatus { get; }
        ITwitterRequestBuilder MakeRequestFor();
        void CheckAuthorization();
        void Deauthorize();
    }
}