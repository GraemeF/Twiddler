using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Core.Commands;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.TweetSharp
{
    [Singleton(typeof (IAuthorizeCommand))]
    [Export(typeof (IAuthorizeCommand))]
    public class AuthorizeCommand : AuthorizationCommand, IAuthorizeCommand
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly ITwitterApplicationCredentials _applicationCredentials;

        [ImportingConstructor]
        public AuthorizeCommand(ITwitterApplicationCredentials applicationCredentials,
                                ITwitterClient client,
                                IAccessTokenStore accessTokenStore)
            : base(client, AuthorizationStatus.Unauthorized)
        {
            _accessTokenStore = accessTokenStore;
            _applicationCredentials = applicationCredentials;
        }

        #region IAuthorizeCommand Members

        [NoCoverage]
        public override void Execute(object parameter)
        {
            var dlg = new OAuthDialog(_accessTokenStore, _applicationCredentials);
            bool? result = dlg.ShowDialog();

            if (result.HasValue == result.Value)
                Client.CheckAuthorization();
        }

        #endregion
    }
}