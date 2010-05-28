using System;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetRequester : IDisposable
    {
        event EventHandler<NewTweetsEventArgs> NewTweets;
    }
}