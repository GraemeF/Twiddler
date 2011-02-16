namespace Twiddler.TwitterStore.Tests
{
    #region Using Directives

    using NSubstitute;

    using Raven.Client;

    using Twiddler.Core.Models;
    using Twiddler.TwitterStore.Interfaces;

    using Xunit;

    #endregion

    public class CredentialsDocumentStoreTests
    {
        private readonly IDocumentSession _documentSession = Substitute.For<IDocumentSession>();

        private readonly IDocumentStore _documentStore = Substitute.For<IDocumentStore>();

        private readonly IDocumentStoreFactory _documentStoreFactory = Substitute.For<IDocumentStoreFactory>();

        public CredentialsDocumentStoreTests()
        {
            _documentStore.OpenSession().Returns(_documentSession);
            _documentStoreFactory.GetDocumentStore().Returns(_documentStore);
        }

        [Fact]
        public void Load_WhenTheCredentialsAreInTheStore_ReturnsTheCredentials()
        {
            const string id = "The credentials id";
            var credentials = new AccessToken(id, null, null);

            AccessTokenDocumentStore test = BuildDefaultTestSubject();

            _documentSession.Load<AccessToken>(id).Returns(credentials);

            Assert.Same(credentials, test.Load(id));
        }

        [Fact]
        public void Load_WhenTheCredentialsAreNotFoundInTheStore_ReturnsNewCredentials()
        {
            const string id = "The credentials id";

            AccessTokenDocumentStore test = BuildDefaultTestSubject();

            _documentSession.Load<AccessToken>(id).Returns((AccessToken)null);

            Assert.NotNull(test.Load(id));
        }

        private AccessTokenDocumentStore BuildDefaultTestSubject()
        {
            return new AccessTokenDocumentStore(_documentStoreFactory);
        }
    }
}