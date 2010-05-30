using System;
using TweetSharp.Twitter.Model;

namespace Twiddler.Tests
{
    public class New
    {
        public static TwitterStatus Tweet
        {
            get
            {
                return new TwitterStatus
                           {
                               Text = "Unspecified Status",
                               Id = 1,
                               User = User,
                               CreatedDate = DateTime.Now.AddMinutes(-5.0)
                           };
            }
        }

        public static TwitterUser User
        {
            get
            {
                return new TwitterUser
                           {
                               Id = 2,
                               Name = "Unspecified Name",
                               ProfileImageUrl = "http://unspecified.url/",
                               ScreenName = "Unspecified Screen Name"
                           };
            }
        }
    }
}