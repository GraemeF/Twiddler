using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetResolver
    {
        ITweet GetTweet(string id);
    }
}