using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Commands.Interfaces;
using Twiddler.Core;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [Singleton(typeof (IAuthorizeCommand))]
    [Export(typeof (IAuthorizeCommand))]
    public class AuthorizeCommand : AuthorizationCommand, IAuthorizeCommand
    {
        [ImportingConstructor]
        public AuthorizeCommand(ITwitterClient client) : base(client, AuthorizationStatus.NotAuthorized)
        {
        }

        #region IAuthorizeCommand Members

        [NoCoverage]
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