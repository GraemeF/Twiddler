using Moq;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.TwitterStore.Interfaces;
using Xunit;

namespace Twiddler.TwitterStore.Tests
{
    public class CredentialsDocumentStoreTests
    {
        private readonly Mock<IDocumentSession> _fakeDocumentSession = new Mock<IDocumentSession>();
        private readonly Mock<IDocumentStore> _fakeDocumentStore = new Mock<IDocumentStore>();
        private readonly Mock<IDocumentStoreFactory> _fakeDocumentStoreFactory = new Mock<IDocumentStoreFactory>();
        
        public CredentialsDocumentStoreTests()
        {
            _fakeDocumentStoreFactory.
                Setup(x => x.GetDocumentStore()).
                Returns(_fakeDocumentStore.Object);

            _fakeDocumentStore.
                Setup(x => x.OpenSession()).
                Returns(_fakeDocumentSession.Object);
        }

        [Fact]
        public void Load_WhenTheCredentialsAreInTheStore_ReturnsTheCredentials()
        {
            string id = "The credentials id";
            var credentials = new TwitterCredentials(id, null, null);

            CredentialsDocumentStore test = BuildDefaultTestSubject();

            _fakeDocumentSession.
                Setup(x => x.Load<TwitterCredentials>(id)).
                Returns(credentials);

            Assert.Same(credentials, test.Load(id));
        }

        [Fact]
        public void Load_WhenTheCredentialsAreNotFoundInTheStore_ReturnsNewCredentials()
        {
            string id = "The credentials id";

            CredentialsDocumentStore test = BuildDefaultTestSubject();

            _fakeDocumentSession.
                Setup(x => x.Load<TwitterCredentials>(id)).
                Returns((TwitterCredentials) null);

            Assert.NotNull(test.Load(id));
        }

        private CredentialsDocumentStore BuildDefaultTestSubject()
        {
            return new CredentialsDocumentStore(_fakeDocumentStoreFactory.Object);
        }
    }
}