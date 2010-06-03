using Moq;
using Raven.Client;
using TweetSharp.Twitter.Model;
using Twiddler.Tests;
using Xunit;

namespace Twiddler.TwitterStore.Tests
{
    public class TwitterDocumentStoreTests
    {
        private Mock<IDocumentStore> _fakeDocumentStore;
        private Mock<IDocumentSession> _fakeDocumentSession;

        [Fact]
        public void AddTweet_GivenTweet_AddsTweetToDocumentStore()
        {
            _fakeDocumentStore = new Mock<IDocumentStore>();
            _fakeDocumentSession = new Mock<IDocumentSession>();

            var test = new TwitterDocumentStore(_fakeDocumentStore.Object);
            
            _fakeDocumentStore.
                Setup(x => x.OpenSession()).
                Returns(_fakeDocumentSession.Object);

            TwitterStatus tweet = New.Tweet;
            test.AddTweet(tweet);

            _fakeDocumentSession.Verify(x => x.Store(tweet));
            _fakeDocumentSession.Verify(x => x.Dispose());
        }
    }
}