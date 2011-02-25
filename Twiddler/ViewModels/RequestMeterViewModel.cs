namespace Twiddler.ViewModels
{
    #region Using Directives

    using System;
    using System.Linq;

    using Caliburn.Micro;

    using MvvmFoundation.Wpf;

    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    public class RequestMeterViewModel : Screen, 
                                         IRequestMeterScreen, 
                                         IDisposable
    {
        private readonly IClock _clock;

        private readonly IObservable<long> _elapsedSeconds = Observable.Interval(TimeSpan.FromSeconds(1));

        private readonly IRequestLimitStatus _limitStatus;

        private PropertyObserver<IRequestLimitStatus> _observer;

        private IDisposable _timePassingSubscription;

        public RequestMeterViewModel(IRequestLimitStatus limitStatus, IClock clock)
        {
            _limitStatus = limitStatus;
            _clock = clock;
        }

        ~RequestMeterViewModel()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public int HourlyLimit
        {
            get { return _limitStatus.HourlyLimit; }
        }

        public string PeriodDuration
        {
            get { return FormatTimeSpan(_limitStatus.PeriodDuration); }
        }

        public int RemainingHits
        {
            get { return _limitStatus.RemainingHits; }
        }

        public string RemainingTime
        {
            get { return FormatTimeSpan(GetRemainingTime()); }
        }

        public float UsedHitsFraction
        {
            get
            {
                return
                    Math.Max(0f, 
                             Math.Min(1f, 
                                      1f - RemainingHits / (float)HourlyLimit));
            }
        }

        public float UsedTimeFraction
        {
            get
            {
                return
                    Math.Max(0f, 
                             Math.Min(1f, 
                                      1f - (float)(GetRemainingTime().TotalSeconds /
                                                   _limitStatus.PeriodDuration.TotalSeconds)));
            }
        }

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                _timePassingSubscription.Dispose();
                _timePassingSubscription = null;

                _observer.
                    UnregisterHandler(x => x.HourlyLimit).
                    UnregisterHandler(x => x.RemainingHits);
                _observer = null;
            }

            base.OnDeactivate(close);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _observer =
                new PropertyObserver<IRequestLimitStatus>(_limitStatus).
                    RegisterHandler(x => x.PeriodDuration, y => UpdatePeriodDuration()).
                    RegisterHandler(x => x.HourlyLimit, y => UpdateHourlyLimit()).
                    RegisterHandler(x => x.RemainingHits, y => UpdateRemainingHits());

            UpdateHourlyLimit();
            UpdateRemainingTime();
            UpdateRemainingHits();
            UpdatePeriodDuration();

            _timePassingSubscription = _elapsedSeconds.Subscribe(x => UpdateRemainingTime());
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                if (_timePassingSubscription != null)
                    _timePassingSubscription.Dispose();
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return string.Concat((int)Math.Ceiling(timeSpan.TotalMinutes), "m");
        }

        private TimeSpan GetRemainingTime()
        {
            TimeSpan remainingTime = _limitStatus.PeriodEndTime - _clock.Now;
            return remainingTime < TimeSpan.Zero
                       ? TimeSpan.Zero
                       : remainingTime;
        }

        private void UpdateHourlyLimit()
        {
            NotifyOfPropertyChange(() => HourlyLimit);
            NotifyOfPropertyChange(() => RemainingHits);
            NotifyOfPropertyChange(() => UsedHitsFraction);
        }

        private void UpdatePeriodDuration()
        {
            NotifyOfPropertyChange(() => RemainingTime);
            NotifyOfPropertyChange(() => UsedTimeFraction);
            NotifyOfPropertyChange(() => PeriodDuration);
        }

        private void UpdateRemainingHits()
        {
            NotifyOfPropertyChange(() => RemainingHits);
            NotifyOfPropertyChange(() => UsedHitsFraction);
        }

        private void UpdateRemainingTime()
        {
            NotifyOfPropertyChange(() => RemainingTime);
            NotifyOfPropertyChange(() => UsedTimeFraction);
        }
    }
}