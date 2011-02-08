namespace Twiddler.Core.Commands
{
    #region Using Directives

    using System;
    using System.Windows.Input;

    using MvvmFoundation.Wpf;

    using Twiddler.Core.Services;

    #endregion

    public abstract class AuthorizationCommand : ICommand
    {
        protected readonly IAuthorizer Client;

        private readonly AuthorizationStatus _executableStatus;

        private PropertyObserver<IAuthorizer> _observer;

        protected AuthorizationCommand(IAuthorizer client, AuthorizationStatus executableStatus)
        {
            Client = client;
            _executableStatus = executableStatus;

            _observer = new PropertyObserver<IAuthorizer>(Client).
                RegisterHandler(x => x.AuthorizationStatus, 
                                y => OnCanExecuteChanged());
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #region ICommand members

        public bool CanExecute(object parameter)
        {
            return Client.AuthorizationStatus == _executableStatus;
        }

        public abstract void Execute(object parameter);

        #endregion

        private void OnCanExecuteChanged()
        {
            Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => CanExecuteChanged(this, EventArgs.Empty));
        }
    }
}