namespace Twiddler.Core.Services
{
    public interface ITwitterRequestBuilder
    {
        ITwitterResult Request();
        ITwitterRequestBuilder Statuses();
        ITwitterRequestBuilder OnFriendsTimeline();
        ITwitterRequestBuilder Since(long since);
        ITwitterRequestBuilder OnHomeTimeline();
        ITwitterRequestBuilder Mentions();
        ITwitterRequestBuilder RetweetsOfMe();
        ITwitterRequestBuilder OnUserTimeline();
    }

    public interface IRawStatus
    {
        long Id { get; }
    }
}