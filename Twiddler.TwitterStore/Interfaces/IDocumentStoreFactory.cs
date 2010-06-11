using Raven.Client;

namespace Twiddler.TwitterStore.Interfaces
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore CreateDocumentStore();
    }
}