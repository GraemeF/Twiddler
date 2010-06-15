using System;
using System.Collections.ObjectModel;

namespace Twiddler.Services.Interfaces
{
    public interface ITimeline : IDisposable
    {
        ObservableCollection<string> Tweets { get; }
    }
}