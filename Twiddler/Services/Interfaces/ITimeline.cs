using System;
using System.Collections.ObjectModel;
using Twiddler.Core.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITimeline : IDisposable
    {
        ObservableCollection<Tweet> Tweets { get; }
    }
}