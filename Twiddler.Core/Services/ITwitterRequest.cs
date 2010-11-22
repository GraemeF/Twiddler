namespace Twiddler.Core.Services
{
    public interface ITwitterRequest
    {
        ITwitterResult GetResponse();
    }

    public interface IRawStatus
    {
        long Id { get; }
    }
}