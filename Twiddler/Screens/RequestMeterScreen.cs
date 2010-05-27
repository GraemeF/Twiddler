using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using MvvmFoundation.Wpf;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IRequestMeterScreen))]
    public class RequestMeterScreen : Screen, IRequestMeterScreen
    {
        private readonly IClock _clock;
        private readonly IRequestLimitStatus _limitStatus;
        private PropertyObserver<IRequestLimitStatus> _observer;

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
    }
}