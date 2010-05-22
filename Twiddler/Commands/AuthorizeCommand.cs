using System;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IAuthorizeCommand))]
    public class AuthorizeCommand : IAuthorizeCommand
    {
        private readonly ITwitterClient _client;
        private PropertyObserver<ITwitterClient> _observer;

        public AuthorizeCommand(ITwitterClient client)
        {
            _client = client;

            _observer = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => CanExecuteChanged(this, EventArgs.Empty));
        }

        #region IAuthorizeCommand Members

        public void Execute(object parameter)
        {
            var dlg = new OAuthDialog();
            bool? result = dlg.ShowDialog();

            if (result.HasValue == result.Value)
                _client.CheckAuthorization();
        }

        public bool CanExecute(object parameter)
        {
            return _client.AuthorizationStatus == AuthorizationStatus.NotAuthorized;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}