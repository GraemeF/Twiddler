using System;
using Twiddler.Core.Services;

namespace Twiddler.Services.Interfaces
{
    public interface IRequestConductor : IDisposable
    {
        void Start(ITweetSink tweetSink);
    }
}