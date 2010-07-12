using Moq;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.TestData;
using Twiddler.TwitterStore.Interfaces;
using Xunit;

namespace Twiddler.TwitterStore.Tests
{
    public class TwitterDocumentStoreTests
    {
        private readonly Mock<IDocumentSession> _fakeDocumentSession = new Mock<IDocumentSession>();
        private readonly Mock<IDocumentStore> _fakeDocumentStore = new Mock<IDocumentStore>();
        private readonly Mock<IDocumentStoreFactory> _fakeDocumentStoreFactory = new Mock<IDocumentStoreFactory>();

        public TwitterDocumentStoreTests()
        {
            _fakeDocumentStoreFactory.
                Setup(x => x.CreateDocumentStore()).
                Returns(_fakeDocumentStore.Object);

            _fakeDocumentStore.
                Setup(x => x.OpenSession()).
                Returns(_fakeDocumentSession.Object);
        }

        [Fact]
        public void Add_GivenTweet_AddsTweetToDocumentStore()
        {
            var test = new TwitterDocumentStore(_fakeDocumentStoreFactory.Object);

            Tweet tweet = A.Tweet;
            test.Add(tweet);

            _fakeDocumentSession.Verify(x => x.Store(tweet));
            _fakeDocumentSession.Verify(x => x.Dispose());
        }

        [Fact]
        public void Add_GivenTweet_RaisesUpdated()
        {
            var test = new TwitterDocumentStore(_fakeDocumentStoreFactory.Object);

            bool eventRaised = false;
            test.Updated += (sender, args) => eventRaised = true;
            test.Add(A.Tweet);

            Assert.True(eventRaised);
        }

        [Fact]
        public void GetTweet_GivenTweetThatIsInTheStore_ReturnsTheTweet()
        {
            Tweet tweet = A.Tweet;
            string id = "The tweet id";

            var test = new TwitterDocumentStore(_fakeDocumentStoreFactory.Object);

            _fakeDocumentSession.
                Setup(x => x.Load<Tweet>(id)).
                Returns(tweet);

            Assert.Same(tweet, test.GetTweet(id));
        }
    }
}