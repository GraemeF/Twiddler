namespace Twiddler.Services
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Caliburn.Core.IoC;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(ITimeline))]
    [Export(typeof(ITimeline))]
    public class StoreTimeline : ITimeline
    {
        private readonly object _mutex = new object();

        private readonly ITweetStore _tweetStore;

        [ImportingConstructor]
        public StoreTimeline(ITweetStore tweetStore)
        {
            _tweetStore = tweetStore;
            Tweets = new ObservableCollection<ITweet>(tweetStore.GetInboxTweets());
        }

        public ObservableCollection<ITweet> Tweets { get; private set; }

        #region ITweetSink members

        public void Add(ITweet tweet)
        {
            _tweetStore.Add(tweet);
            AddNewTweets(new[] { tweet });
        }

        public void Add(IEnumerable<ITweet> tweets)
        {
            _tweetStore.Add(tweets);
            AddNewTweets(tweets);
        }

        #endregion

        private void AddNewTweets(IEnumerable<ITweet> tweets)
        {
            lock (_mutex)
                foreach (ITweet tweet in tweets.Where(tweet => !Tweets.Any(x => x.Id == tweet.Id)))
                    Tweets.Add(tweet);
        }
    }
}