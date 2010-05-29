using System;

namespace Twiddler.Models
{
    public class User
    {
        public UserId Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string ProfileImageUrl { get; set; }
        public int FollowersCount { get; set; }
        public bool IsVerified { get; set; }
    }
}