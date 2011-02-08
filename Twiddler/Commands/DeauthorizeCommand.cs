namespace Twiddler.Commands
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Services;

    #endregion

    [PerRequest(typeof(IDeauthorizeCommand))]
    [Export(typeof(IDeauthorizeCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DeauthorizeCommand : AuthorizationCommand, 
                                      IDeauthorizeCommand
    {
        [ImportingConstructor]
        public DeauthorizeCommand(IAuthorizer client)
            : base(client, AuthorizationStatus.Authorized)
        {
        }

        #region ICommand members

        public override void Execute(object parameter)
        {
            Client.Deauthorize();
        }

        #endregion
    }
}