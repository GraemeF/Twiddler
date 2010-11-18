using System;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    public abstract class AuthorizationCommand : ICommand
    {
        protected readonly ITwitterClient Client;
        private readonly AuthorizationStatus _executableStatus;
        private PropertyObserver<ITwitterClient> _observer;

        protected AuthorizationCommand(ITwitterClient client, AuthorizationStatus executableStatus)
        {
            Client = client;
            _executableStatus = executableStatus;

            _observer = new PropertyObserver<ITwitterClient>(Client).
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