namespace Twiddler.Core.Models
{
    #region Using Directives

    using System;

    using ReactiveUI;

    #endregion

    public interface IRequestLimitStatus : IReactiveNotifyPropertyChanged
    {
        int HourlyLimit { get; set; }

        TimeSpan PeriodDuration { get; set; }

        DateTime PeriodEndTime { get; set; }

        int RemainingHits { get; set; }
    }
}