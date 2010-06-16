using Twiddler.Core.Services;

namespace Twiddler.Services.Interfaces
{
    public interface ISelfUpdatingTweetStore : ITweetResolver, ITweetSource
    {
    }
}