using Twiddler.Core.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Services;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class TweetRatingTests
    {
        private Tweet _tweet;
        private IUserInfo _userInfo;
        
        [Fact]
        public void GettingIsMention_WhenTheUserIsNotMentioned_ReturnsFalse()
        {
            _tweet = A.Tweet;
            var test = BuildDefaultTestSubject();
        }

        private TweetRating BuildDefaultTestSubject()
        {
            return new TweetRating(_userInfo, _tweet);
        }
    }
}