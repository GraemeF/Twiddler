using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface IAccessTokenStore
    {
        IAccessToken Load(string id);
        void Save(IAccessToken accessToken);
    }
}