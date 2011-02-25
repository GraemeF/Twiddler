namespace Twiddler.Core.Commands
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Windows.Input;

    using ReactiveUI;

    using Twiddler.Core.Services;

    #endregion

    public abstract class AuthorizationCommand : ICommand
    {
        protected readonly IAuthorizer Client;

        private readonly AuthorizationStatus _executableStatus;

        private IDisposable _observer;

        protected AuthorizationCommand(IAuthorizer client, AuthorizationStatus executableStatus)
        {
            Client = client;
            _executableStatus = executableStatus;

            _observer = client.
                WhenAny(x => x.AuthorizationStatus, _ => true).
                ObserveOnDispatcher().
                Subscribe(Observer.Create<bool>(_ => CanExecuteChanged(this, EventArgs.Empty)));
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #region ICommand members

        public bool CanExecute(object parameter)
        {
            return Client.AuthorizationStatus == _executableStatus;
        }

        public abstract void Execute(object parameter);

        #endregion
    }
}