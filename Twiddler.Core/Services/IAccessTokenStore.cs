namespace Twiddler.Core.Services
{
    #region Using Directives

    using Twiddler.Core.Models;

    #endregion

    public interface IAccessTokenStore
    {
        AccessToken Load(string id);

        void Save(AccessToken accessToken);
    }
}