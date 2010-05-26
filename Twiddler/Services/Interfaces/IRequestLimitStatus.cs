using System;
using System.ComponentModel;

namespace Twiddler.Services.Interfaces
{
    public interface IRequestLimitStatus : INotifyPropertyChanged
    {
        int HourlyLimit { get; set; }
        int RemainingHits { get; set; }
        DateTime PeriodEndTime { get; set; }
        TimeSpan PeriodDuration { get; set; }
    }
}