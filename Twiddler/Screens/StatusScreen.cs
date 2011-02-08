namespace Twiddler.Screens
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.Linq;

    using Caliburn.Core.IoC;
    using Caliburn.PresentationFramework.Screens;

    using MvvmFoundation.Wpf;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Services;
    using Twiddler.Screens.Interfaces;

    #endregion

    [Singleton(typeof(IStatusScreen))]
    [Export(typeof(IStatusScreen))]
    public class StatusScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, 
                                IStatusScreen
    {
        private readonly IAuthorizer _client;

        private PropertyObserver<IAuthorizer> _observer;

        [ImportingConstructor]
        public StatusScreen(IAuthorizer client, 
                            IAuthorizeCommand authorizeCommand, 
                            IDeauthorizeCommand deauthorizeCommand, 
                            IRequestMeterScreen requestMeter)
            : base(true)
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

            _observer = new PropertyObserver<IAuthorizer>(_client).
                RegisterHandler(x => x.AuthorizationStatus, 
                                y => NotifyOfPropertyChange(() => Authorization));

            Observable.Start(_client.CheckAuthorization);
        }
    }
}