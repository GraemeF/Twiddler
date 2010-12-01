using System.Linq;
using Moq;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.TwitterStore.Interfaces;
using Xunit;

namespace Twiddler.TwitterStore.Tests
{
    public class CredentialsDocumentStoreTests
    {
        private readonly IDocumentSession _documentSession = Mock.Of<IDocumentSession>();
        private readonly IDocumentStore _documentStore;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public CredentialsDocumentStoreTests()
        {
            _documentStore = Mocks.Of<IDocumentStore>().First(x => x.OpenSession() == _documentSession);
            _documentStoreFactory = Mocks.Of<IDocumentStoreFactory>().First(x => x.GetDocumentStore() == _documentStore);
        }

        [Fact]
        public void Load_WhenTheCredentialsAreInTheStore_ReturnsTheCredentials()
        {
            const string id = "The credentials id";
            var credentials = new AccessToken(id, null, null);

            AccessTokenDocumentStore test = BuildDefaultTestSubject();

            Mock.Get(_documentSession).
                Setup(x => x.Load<AccessToken>(id)).
                Returns(credentials);

            Assert.Same(credentials, test.Load(id));
        }

        [Fact]
        public void Load_WhenTheCredentialsAreNotFoundInTheStore_ReturnsNewCredentials()
        {
            const string id = "The credentials id";

            AccessTokenDocumentStore test = BuildDefaultTestSubject();

            Mock.Get(_documentSession).
                Setup(x => x.Load<AccessToken>(id)).
                Returns((AccessToken) null);

            Assert.NotNull(test.Load(id));
        }

        private AccessTokenDocumentStore BuildDefaultTestSubject()
        {
            return new AccessTokenDocumentStore(_documentStoreFactory);
        }
    }
}