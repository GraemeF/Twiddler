using System;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetPoller : IDisposable
    {
        event EventHandler<NewTweetsEventArgs> NewTweets;
    }
}