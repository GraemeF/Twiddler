using System;
using System.Collections.ObjectModel;

namespace Twiddler.Models.Interfaces
{
    public interface ITimeline : IDisposable
    {
        ObservableCollection<Tweet> Tweets { get; }
    }
}