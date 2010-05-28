using System;

namespace Twiddler.Services.Interfaces
{
    public interface IRequestConductor : IDisposable
    {
        void Start(ITweetSink tweetSink);
    }
}