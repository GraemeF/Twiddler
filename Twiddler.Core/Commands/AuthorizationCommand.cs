using System;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using Twiddler.Core.Services;

namespace Twiddler.Core.Commands
{
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

        #region ICommand Members

        public abstract void Execute(object parameter);

        public bool CanExecute(object parameter)
        {
            return Client.AuthorizationStatus == _executableStatus;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion

        private void OnCanExecuteChanged()
        {
            Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => CanExecuteChanged(this, EventArgs.Empty));
        }
    }
}