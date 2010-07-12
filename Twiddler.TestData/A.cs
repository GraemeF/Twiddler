using Twiddler.Core.Models;

namespace Twiddler.TestData
{
    public class A
    {
        public static TweetBuilder Tweet
        {
            get { return new TweetBuilder(); }
        }

        public static User User
        {
            get
            {
                return new User
                           {
                               Id = "2",
                               Name = "Unspecified Name",
                               ProfileImageUrl = "http://unspecified.url/",
                               ScreenName = "Unspecified Screen Name"
                           };
            }
        }
    }
}