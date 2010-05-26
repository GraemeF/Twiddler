using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IRequestMeterScreen))]
    public class RequestMeterScreen : Screen, IRequestMeterScreen
    {
        private readonly IRequestStatus _status;

        public RequestMeterScreen(IRequestStatus status)
        {
            _status = status;
        }

        public int HourlyLimit
        {
            get { return _status.HourlyLimit; }
        }

        public int RemainingHits
        {
            get { return _status.RemainingHits; }
        }

        public float UsedHitsFraction { get; private set; }
        public float UsedTimeFraction { get; private set; }
        public string RemainingTime { get; private set; }
    }
}