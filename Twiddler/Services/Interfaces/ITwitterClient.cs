using System.ComponentModel;
using TweetSharp.Twitter.Fluent;
using Twiddler.Core.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITwitterClient : INotifyPropertyChanged
    {
        User AuthenticatedUser { get; }
        AuthorizationStatus AuthorizationStatus { get; }
        IFluentTwitter MakeRequestFor();
        void CheckAuthorization();
        void Deauthorize();
    }
}