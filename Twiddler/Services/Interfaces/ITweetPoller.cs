using System;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetPoller : IDisposable
    {
        void Start();
        void Stop();
        event EventHandler<NewTweetsEventArgs> NewTweets;
    }
}