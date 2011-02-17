namespace Twiddler.Commands
{
    #region Using Directives

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Services;

    #endregion

    public class DeauthorizeCommand : AuthorizationCommand, 
                                      IDeauthorizeCommand
    {
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