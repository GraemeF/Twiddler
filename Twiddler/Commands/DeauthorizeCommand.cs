using Caliburn.Core.IoC;
using Twiddler.Commands.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IDeauthorizeCommand))]
    public class DeauthorizeCommand : AuthorizationCommand, IDeauthorizeCommand
    {
        public DeauthorizeCommand(ITwitterClient client)
            : base(client, AuthorizationStatus.Authorized)
        {
        }

        #region IDeauthorizeCommand Members

        public override void Execute(object parameter)
        {
            Client.Deauthorize();
        }

        #endregion
    }
}