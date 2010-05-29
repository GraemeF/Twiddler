using Moq;
using Twiddler.Models;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class LoadingTweetScreenTests
    {
        private readonly Tweet _tweet = New.Tweet;

        [Fact]
        public void GettingId__ReturnsTweetId()
        {
            var test = new LoadingTweetScreen(new Mock<IUpdatingTweetStore>().Object, _tweet.Id, x => new Mock<ITweetScreen>().Object);

            Assert.Equal(_tweet.Id, test.Id);
        }
    }
}