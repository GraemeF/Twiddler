using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface IAccessTokenStore
    {
        AccessToken Load(string id);
        void Save(AccessToken accessToken);
    }
}