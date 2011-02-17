namespace Twiddler.Screens
{
    #region Using Directives

    using System;
    using System.Linq;

    using Caliburn.PresentationFramework.Screens;

    using ReactiveUI;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Services;
    using Twiddler.Screens.Interfaces;

    #endregion

    public class StatusScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, 
                                IStatusScreen
    {
        private readonly IAuthorizer _client;

        private IDisposable _observer;

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

            _observer = _client.
                ObservableForProperty(x => x.AuthorizationStatus).
                Subscribe(_ => NotifyOfPropertyChange(() => Authorization));

            Observable.Start(_client.CheckAuthorization);
        }
    }
}