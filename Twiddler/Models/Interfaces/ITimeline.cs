using System;
using System.Collections.ObjectModel;
using Twiddler.Core.Models;

namespace Twiddler.Models.Interfaces
{
    public interface ITimeline : IDisposable
    {
        ObservableCollection<TweetId> Tweets { get; }
    }
}