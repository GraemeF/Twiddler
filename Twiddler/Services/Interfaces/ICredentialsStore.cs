using Twiddler.Models.Interfaces;

namespace Twiddler.Services.Interfaces
{
    public interface ICredentialsStore
    {
        ITwitterCredentials Load();
        void Save(ITwitterCredentials credentials);
    }
}