namespace Twiddler.TwitterStore.Interfaces
{
    #region Using Directives

    using Raven.Client;

    #endregion

    public interface IDocumentStoreFactory
    {
        IDocumentStore GetDocumentStore();
    }
}