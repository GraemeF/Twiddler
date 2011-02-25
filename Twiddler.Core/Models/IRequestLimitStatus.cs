namespace Twiddler.Core.Models
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    #endregion

    public interface IRequestLimitStatus : INotifyPropertyChanged
    {
        int HourlyLimit { get; set; }

        TimeSpan PeriodDuration { get; set; }

        DateTime PeriodEndTime { get; set; }

        int RemainingHits { get; set; }
    }
}