namespace Twiddler.Services
{
    #region Using Directives

    using System;

    using ReactiveUI;

    using Twiddler.Core.Models;
    using Twiddler.Services.Interfaces;

    #endregion

    public class RequestLimitStatus : ReactiveObject, 
                                      IRequestLimitStatus
    {
        private readonly TimeSpan _PeriodDuration = TimeSpan.FromHours(1);

        private int _HourlyLimit = 350;

        private DateTime _PeriodEndTime;

        private int _RemainingHits;

        public int HourlyLimit
        {
            get { return _HourlyLimit; }
            set { this.RaiseAndSetIfChanged(x => x.HourlyLimit, value); }
        }

        public TimeSpan PeriodDuration
        {
            get { return _PeriodDuration; }
            set { this.RaiseAndSetIfChanged(x => x.PeriodDuration, value); }
        }

        public DateTime PeriodEndTime
        {
            get { return _PeriodEndTime; }
            set { this.RaiseAndSetIfChanged(x => x.PeriodEndTime, value); }
        }

        public int RemainingHits
        {
            get { return _RemainingHits; }
            set { this.RaiseAndSetIfChanged(x => x.RemainingHits, value); }
        }
    }
}