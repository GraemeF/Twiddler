using System;
using Twiddler.Models.Interfaces;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetSource
    {
        IObservable<ITweet> Tweets { get; }
    }
}