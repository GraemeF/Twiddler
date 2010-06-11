using Moq;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.Tests;
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
        public void AddTweet_GivenTweet_AddsTweetToDocumentStore()
        {
            var test = new TwitterDocumentStore(_fakeDocumentStoreFactory.Object);

            Tweet tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeDocumentSession.Verify(x => x.Store(tweet));
            _fakeDocumentSession.Verify(x => x.Dispose());
        }
    }
}