using System;
using System.Collections.Generic;

namespace Twiddler.Models
{
    public class Tweet
    {
        public Tweet()
        {
            Links = new Uri[] {};
        }

        public TweetId Id { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string InReplyToStatusId { get; set; }
        public IEnumerable<Uri> Links { get; set; }
    }
}