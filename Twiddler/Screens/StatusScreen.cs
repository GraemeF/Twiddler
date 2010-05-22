using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (IStatusScreen))]
    public class StatusScreen : Screen, IStatusScreen
    {
        private readonly ITwitterClient _client;
        private PropertyObserver<ITwitterClient> _observer;

        public StatusScreen(ITwitterClient client, IAuthorizeCommand authorizeCommand)
        {
            _client = client;
            AuthorizeCommand = authorizeCommand;
        }

        public AuthorizationStatus Authorization
        {
            get { return _client.AuthorizationStatus; }
        }

        public IAuthorizeCommand AuthorizeCommand { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _observer = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => NotifyOfPropertyChange(() => Authorization));

            _client.CheckAuthorization();
        }
    }
}