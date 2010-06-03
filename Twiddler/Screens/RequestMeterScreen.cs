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
                                      1f - (float) (GetRemainingSeconds()/_limitStatus.PeriodDuration.TotalSeconds)));
            }
        }

        public string RemainingTime { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private double GetRemainingSeconds()
        {
            return (_limitStatus.PeriodEndTime - _clock.Now).TotalSeconds;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _observer =
                new PropertyObserver<IRequestLimitStatus>(_limitStatus).
                    RegisterHandler(x => x.HourlyLimit, y => HourlyLimitChanged()).
                    RegisterHandler(x => x.RemainingHits, y => RemainingHitsChanged());

            _timePassingSubscription = _elapsedSeconds.Subscribe(x => TimePassed());
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
            NotifyOfPropertyChange(() => UsedTimeFraction);
        }

        private void RemainingHitsChanged()
        {
            NotifyOfPropertyChange(() => RemainingHits);
            NotifyOfPropertyChange(() => UsedHitsFraction);
        }

        private void HourlyLimitChanged()
        {
            NotifyOfPropertyChange(() => HourlyLimit);
            NotifyOfPropertyChange(() => UsedHitsFraction);
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