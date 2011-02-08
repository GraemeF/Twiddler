namespace Twiddler.Core.Models
{
    public class User
    {
        public int FollowersCount { get; set; }

        public string Id { get; set; }

        public bool IsVerified { get; set; }

        public string Name { get; set; }

        public string ProfileImageUrl { get; set; }

        public string ScreenName { get; set; }
    }
}