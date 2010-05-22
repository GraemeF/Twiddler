using Caliburn.Core.IoC;
using Twiddler.Commands.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IAuthorizeCommand))]
    public class AuthorizeCommand : AuthorizationCommand, IAuthorizeCommand
    {
        public AuthorizeCommand(ITwitterClient client) : base(client, AuthorizationStatus.NotAuthorized)
        {
        }

        #region IAuthorizeCommand Members

        public override void Execute(object parameter)
        {
            var dlg = new OAuthDialog();
            bool? result = dlg.ShowDialog();

            if (result.HasValue == result.Value)
                Client.CheckAuthorization();
        }

        #endregion
    }
}