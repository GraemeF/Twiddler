namespace Twiddler.Models
{
    public class Tweet
    {
        public long Id { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
    }
}