using System.ComponentModel;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
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