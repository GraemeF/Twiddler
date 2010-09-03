using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Commands.Interfaces;
using Twiddler.Core;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Commands
{
    [Singleton(typeof (IAuthorizeCommand))]
    [Export(typeof (IAuthorizeCommand))]
    public class AuthorizeCommand : AuthorizationCommand, IAuthorizeCommand
    {
        private readonly ICredentialsStore _credentialsStore;

        [ImportingConstructor]
        public AuthorizeCommand(ITwitterClient client, ICredentialsStore credentialsStore)
            : base(client, AuthorizationStatus.Unauthorized)
        {
            _credentialsStore = credentialsStore;
        }

        #region IAuthorizeCommand Members

        [NoCoverage]
        public override void Execute(object parameter)
        {
            var dlg = new OAuthDialog(_credentialsStore);
            bool? result = dlg.ShowDialog();

            if (result.HasValue == result.Value)
                Client.CheckAuthorization();
        }

        #endregion
    }
}