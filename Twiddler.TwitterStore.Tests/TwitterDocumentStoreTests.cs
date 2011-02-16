namespace Twiddler.TwitterStore.Tests
{
    #region Using Directives

    using NSubstitute;

    using Raven.Client;

    using Should.Fluent;

    using Twiddler.TwitterStore.Interfaces;
    using Twiddler.TwitterStore.Models;

    using Xunit;

    #endregion

    public class TwitterDocumentStoreTests
    {
        private readonly IDocumentSession _documentSession = Substitute.For<IDocumentSession>();

        private readonly IDocumentStore _documentStore = Substitute.For<IDocumentStore>();

        private readonly IDocumentStoreFactory _documentStoreFactory = Substitute.For<IDocumentStoreFactory>();

        public TwitterDocumentStoreTests()
        {
            _documentStoreFactory.GetDocumentStore().Returns(_documentStore);

            _documentStore.OpenSession().Returns(_documentSession);
        }

        [Fact]
        public void Add_GivenTweet_AddsTweetToDocumentStore()
        {
            var test = new TwitterDocumentStore(_documentStoreFactory);

            var tweet = new Tweet();
            test.Add(tweet);

            _documentSession.Received().Store(tweet);
            _documentSession.Received().Dispose();
        }

        [Fact]
        public void GetTweet_GivenTweetThatIsInTheStore_ReturnsTheTweet()
        {
            var tweet = new Tweet();
            string id = "The tweet id";

            var test = new TwitterDocumentStore(_documentStoreFactory);

            _documentSession.Load<Tweet>(id).Returns(tweet);

            test.GetTweet(id).Should().Be.SameAs(tweet);
        }
    }
}