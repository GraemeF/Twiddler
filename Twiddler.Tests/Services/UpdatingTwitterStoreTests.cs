using System.Collections.Generic;
using Moq;
using Twiddler.Models;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class UpdatingTwitterStoreTests
    {
        private readonly Mock<ITweetSource> _stubSource = new Mock<ITweetSource>();
        private readonly Subject<Tweet> _tweets = new Subject<Tweet>();

        public UpdatingTwitterStoreTests()
        {
            _stubSource.Setup(x => x.Tweets).Returns(_tweets);
        }

        [Fact]
        public void GettingTweets__ReturnsTweetsFromSource()
        {
            UpdatingTwitterStore test = BuildDefaultTestSubject();

            Assert.Same(_tweets, test.NewTweets);
        }

        private UpdatingTwitterStore BuildDefaultTestSubject()
        {
            return new UpdatingTwitterStore(_stubSource.Object);
        }
    }
}