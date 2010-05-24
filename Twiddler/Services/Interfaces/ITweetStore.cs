using System.Collections.ObjectModel;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetStore
    {
        ObservableCollection<Tweet> AllTweets { get; }
        void AddTweet(Tweet tweet);
        Tweet GetTweet(TweetId id);
    }
}