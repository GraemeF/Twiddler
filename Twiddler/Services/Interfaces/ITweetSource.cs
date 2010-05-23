using System;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<Tweet> Tweets { get; }
    }
}