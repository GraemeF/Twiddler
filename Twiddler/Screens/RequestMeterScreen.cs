using System;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using MvvmFoundation.Wpf;
using TweetSharp.Extensions;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IRequestMeterScreen))]
    public class RequestMeterScreen : Screen, IRequestMeterScreen, IDisposable
    {
        private readonly IClock _clock;
        private readonly IObservable<long> _elapsedSeconds = Observable.Interval(1.Second());
        private readonly IRequestLimitStatus _limitStatus;
        private PropertyObserver<IRequestLimitStatus> _observer;
        private IDisposable _timePassingSubscription;

        public RequestMeterScreen(IRequestLimitStatus limitStatus, IClock clock)
        {
            _limitStatus = limitStatus;
            _clock = clock;
        }

        public int HourlyLimit
        {
            get { return _limitStatus.HourlyLimit; }
        }

        public int RemainingHits
        {
            get { return _limitStatus.RemainingHits; }
        }

        public float UsedHitsFraction
        {
            get
            {
                return
                    Math.Max(0f,
                             Math.Min(1f,
                                      1f - RemainingHits/(float) HourlyLimit));
            }
        }

        public float UsedTimeFraction
        {
            get
            {
                return
                    Math.Max(0f,
                             Math.Min(1f,
                                      1f - (float) (GetRemainingTime().TotalSeconds/
                                                    _limitStatus.PeriodDuration.TotalSeconds)));
            }
        }

        public string RemainingTime
        {
            get { return FormatTimeSpan(GetRemainingTime()); }
        }

        public string PeriodDuration
        {
            get { return FormatTimeSpan(_limitStatus.PeriodDuration); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return string.Concat((int) Math.Ceiling(timeSpan.TotalMinutes), "m");
        }

        private TimeSpan GetRemainingTime()
        {
            TimeSpan remainingTime = _limitStatus.PeriodEndTime - _clock.Now;
            return remainingTime < TimeSpan.Zero
                       ? TimeSpan.Zero
                       : remainingTime;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _observer =
                new PropertyObserver<IRequestLimitStatus>(_limitStatus).
                    RegisterHandler(x => x.HourlyLimit, y => HourlyLimitChanged()).
                    RegisterHandler(x => x.RemainingHits, y => RemainingHitsChanged());

            UpdateHourlyLimit();
            UpdateRemainingTime();
            UpdateRemainingHits();
            UpdatePeriodDuration();

            _timePassingSubscription = _elapsedSeconds.Subscribe(x => TimePassed());
            TimePassed();
        }

        private void UpdatePeriodDuration()
        {
            NotifyOfPropertyChange(() => RemainingTime);
            NotifyOfPropertyChange(() => UsedTimeFraction);
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

        private void UpdateHourlyLimit()
        {
            NotifyOfPropertyChange(() => HourlyLimit);
            NotifyOfPropertyChange(() => RemainingHits);
            NotifyOfPropertyChange(() => UsedHitsFraction);
        }

        protected override void OnShutdown()
        {
            _timePassingSubscription.Dispose();
            _timePassingSubscription = null;

            _observer.
                UnregisterHandler(x => x.HourlyLimit).
                UnregisterHandler(x => x.RemainingHits);
            _observer = null;

            base.OnShutdown();
        }

        private void TimePassed()
        {
            UpdateRemainingTime();
        }

        private void RemainingHitsChanged()
        {
            UpdateRemainingHits();
        }

        private void HourlyLimitChanged()
        {
            UpdateHourlyLimit();
        }

        ~RequestMeterScreen()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                if (_timePassingSubscription != null)
                    _timePassingSubscription.Dispose();
        }
    }
}