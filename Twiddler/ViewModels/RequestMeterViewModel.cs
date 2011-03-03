namespace Twiddler.ViewModels
{
    #region Using Directives

    using System;
    using System.Linq;

    using Caliburn.Micro;

    using ReactiveUI;

    using Twiddler.Core.Models;
    using Twiddler.Services.Interfaces;
    using Twiddler.ViewModels.Interfaces;

    #endregion

    public class RequestMeterViewModel : Screen, 
                                         IRequestMeterScreen, 
                                         IDisposable
    {
        private readonly IClock _clock;

        private readonly IObservable<long> _elapsedSeconds = Observable.Interval(TimeSpan.FromSeconds(1));

        private readonly IRequestLimitStatus _limitStatus;

        private IDisposable _hourlyLimitSubscription;

        private IDisposable _periodSubscription;

        private IDisposable _remainingHitsSubscription;

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
                _periodSubscription.Dispose();
                _hourlyLimitSubscription.Dispose();
                _remainingHitsSubscription.Dispose();
            }

            base.OnDeactivate(close);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _periodSubscription = _limitStatus.
                WhenAny(x => x.PeriodDuration, _ => true).
                Subscribe(_ => UpdatePeriodDuration());

            _hourlyLimitSubscription = _limitStatus.
                WhenAny(x => x.HourlyLimit, _ => true).
                Subscribe(_ => UpdateHourlyLimit());

            _remainingHitsSubscription = _limitStatus.
                WhenAny(x => x.RemainingHits, _ => true).
                Subscribe(_ => UpdateRemainingHits());

            UpdateHourlyLimit();
            UpdateRemainingTime();
            UpdateRemainingHits();
            UpdatePeriodDuration();

            _timePassingSubscription = _elapsedSeconds.Subscribe(x => UpdateRemainingTime());
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return string.Concat((int)Math.Ceiling(timeSpan.TotalMinutes), "m");
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                if (_timePassingSubscription != null)
                    _timePassingSubscription.Dispose();
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