using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [Singleton(typeof (IStatusScreen))]
    [Export(typeof (IStatusScreen))]
    public class StatusScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, IStatusScreen
    {
        private readonly ITwitterClient _client;
        private PropertyObserver<ITwitterClient> _observer;

        [ImportingConstructor]
        public StatusScreen(ITwitterClient client,
                            IAuthorizeCommand authorizeCommand,
                            IDeauthorizeCommand deauthorizeCommand,
                            IRequestMeterScreen requestMeter) : base(true)
        {
            _client = client;
            AuthorizeCommand = authorizeCommand;
            DeauthorizeCommand = deauthorizeCommand;
            RequestMeter = requestMeter;
        }

        public AuthorizationStatus Authorization
        {
            get { return _client.AuthorizationStatus; }
        }

        public IAuthorizeCommand AuthorizeCommand { get; private set; }
        public IDeauthorizeCommand DeauthorizeCommand { get; private set; }
        public IRequestMeterScreen RequestMeter { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _observer = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => NotifyOfPropertyChange(() => Authorization));

            Observable.Start(_client.CheckAuthorization);
        }
    }
}