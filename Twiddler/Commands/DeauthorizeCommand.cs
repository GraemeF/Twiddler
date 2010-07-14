using System.ComponentModel.Composition;
using Twiddler.Commands.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [Export(typeof (IDeauthorizeCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DeauthorizeCommand : AuthorizationCommand, IDeauthorizeCommand
    {
        [ImportingConstructor]
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