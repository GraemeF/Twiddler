using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using MvvmFoundation.Wpf;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IStatusScreen))]
    public class StatusScreen : Screen, IStatusScreen
    {
        private readonly ITwitterClient _client;
        private PropertyObserver<ITwitterClient> _observer;

        public StatusScreen(ITwitterClient client)
        {
            _client = client;
        }

        public AuthorizationStatus Authorization
        {
            get { return _client.AuthorizationStatus; }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _observer = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => NotifyOfPropertyChange(() => Authorization));
        }
    }
}