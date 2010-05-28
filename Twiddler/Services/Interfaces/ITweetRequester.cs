using System;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetRequester
    {
        void Request();
        event EventHandler<NewTweetsEventArgs> NewTweets;
    }
}