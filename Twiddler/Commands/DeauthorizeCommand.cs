using System;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IDeauthorizeCommand))]
    public class DeauthorizeCommand : IDeauthorizeCommand
    {
        private readonly ITwitterClient _client;
        private PropertyObserver<ITwitterClient> _observer;

        public DeauthorizeCommand(ITwitterClient client)
        {
            _client = client;

            _observer = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => CanExecuteChanged(this, EventArgs.Empty));
        }

        public void Execute(object parameter)
        {
            _client.Deauthorize();
        }

        public bool CanExecute(object parameter)
        {
            return _client.AuthorizationStatus == AuthorizationStatus.Authorized;
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}