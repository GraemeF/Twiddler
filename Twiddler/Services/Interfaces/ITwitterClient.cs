using System.ComponentModel;
using TweetSharp.Twitter.Fluent;

namespace Twiddler.Services.Interfaces
{
    public interface ITwitterClient : INotifyPropertyChanged
    {
        AuthorizationStatus AuthorizationStatus { get; }
        IFluentTwitter MakeRequestFor();
        void CheckAuthorization();
        void Deauthorize();
    }
}