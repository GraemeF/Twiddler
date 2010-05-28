using System;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetRequester
    {
        void Request();
        IObservable<Tweet> Tweets { get; }
    }
}