using System;
using System.ComponentModel;
using Caliburn.Core.IoC;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (IRequestStatus))]
    public class RequestStatus : IRequestStatus
    {
        private int _hourlyLimit;
        private TimeSpan _periodDuration;
        private DateTime _periodEndTime;
        private int _remainingHits;

        #region IRequestStatus Members

        public event PropertyChangedEventHandler PropertyChanged;

        public int HourlyLimit
        {
            get { return _hourlyLimit; }
            set
            {
                if (_hourlyLimit != value)
                {
                    _hourlyLimit = value;
                    PropertyChanged.Raise(x => HourlyLimit);
                }
            }
        }

        public int RemainingHits
        {
            get { return _remainingHits; }
            set
            {
                if (_remainingHits != value)
                {
                    _remainingHits = value;
                    PropertyChanged.Raise(x => RemainingHits);
                }
            }
        }

        public DateTime PeriodEndTime
        {
            get { return _periodEndTime; }
            set
            {
                if (_periodEndTime != value)
                {
                    _periodEndTime = value;
                    PropertyChanged.Raise(x => PeriodEndTime);
                }
            }
        }

        public TimeSpan PeriodDuration
        {
            get { return _periodDuration; }
            set
            {
                if (_periodDuration != value)
                {
                    _periodDuration = value;
                    PropertyChanged.Raise(x => PeriodDuration);
                }
            }
        }

        #endregion
    }
}