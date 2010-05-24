using System;

namespace Twiddler.Models
{
    public class Tweet
    {
        public TweetId Id { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}