using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ICredentialsStore
    {
        ITwitterCredentials Load(string id);
        void Save(ITwitterCredentials credentials);
    }
}