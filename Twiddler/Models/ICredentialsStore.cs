using Twiddler.Models.Interfaces;

namespace Twiddler.Models
{
    public interface ICredentialsStore
    {
        ITwitterCredentials Load();
        void Save(ITwitterCredentials credentials);
    }
}