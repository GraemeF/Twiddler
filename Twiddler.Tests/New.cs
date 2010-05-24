using System;
using Twiddler.Models;

namespace Twiddler.Tests
{
    public class New
    {
        public static Tweet Tweet
        {
            get
            {
                return new Tweet
                           {
                               Status = "Unspecified Status",
                               Id = new TweetId(1),
                               User = User,
                               CreatedDate = DateTime.Now.AddMinutes(-5.0)
                           };
            }
        }

        public static User User
        {
            get
            {
                return new User
                           {
                               Id = new UserId(2),
                               Name = "Unspecified Name",
                               ProfileImageUrl = "http://unspecified.url/",
                               ScreenName = "Unspecified Screen Name"
                           };
            }
        }
    }
}