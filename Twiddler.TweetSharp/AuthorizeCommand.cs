namespace Twiddler.TweetSharp
{
    #region Using Directives

    using Twiddler.Core;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    public class AuthorizeCommand : AuthorizationCommand, 
                                    IAuthorizeCommand
    {
        private readonly IAccessTokenStore _accessTokenStore;

        private readonly ITwitterApplicationCredentials _applicationCredentials;

        public AuthorizeCommand(ITwitterApplicationCredentials applicationCredentials, 
                                IAuthorizer client, 
                                IAccessTokenStore accessTokenStore)
            : base(client, AuthorizationStatus.Unauthorized)
        {
            _accessTokenStore = accessTokenStore;
            _applicationCredentials = applicationCredentials;
        }

        #region ICommand members

        [NoCoverage]
        public override void Execute(object parameter)
        {
            var dlg = new OAuthDialog(_accessTokenStore, _applicationCredentials);
            bool? result = dlg.ShowDialog();

            if (result.HasValue ==
                result.Value)
                Client.CheckAuthorization();
        }

        #endregion
    }
}