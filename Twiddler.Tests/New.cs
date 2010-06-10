using System;
using Twiddler.Core.Models;
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
                               Id = "1",
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
                               Id = "2",
                               Name = "Unspecified Name",
                               ProfileImageUrl = "http://unspecified.url/",
                               ScreenName = "Unspecified Screen Name"
                           };
            }
        }
    }
}