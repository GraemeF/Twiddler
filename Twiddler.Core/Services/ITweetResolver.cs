using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetResolver : ITweetSink
    {
        ITweet GetTweet(string id);
    }
}