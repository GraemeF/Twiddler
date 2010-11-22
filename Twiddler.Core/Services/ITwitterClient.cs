using System.ComponentModel;
using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITwitterClient : INotifyPropertyChanged
    {
        User AuthenticatedUser { get; }
        AuthorizationStatus AuthorizationStatus { get; }
        void CheckAuthorization();
        void Deauthorize();
        ITwitterRequest CreateRequestForStatusesOnFriendsTimeline(long since);
        ITwitterRequest CreateRequestForStatusesOnHomeTimeline(long since);
        ITwitterRequest CreateRequestForMentions(long since);
        ITwitterRequest CreateRequestForRetweetsOfMe(long since);
        ITwitterRequest CreateRequestForStatusesOnUserTimeline(long since);
    }
}