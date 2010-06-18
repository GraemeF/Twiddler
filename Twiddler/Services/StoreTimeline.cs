using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITimeline))]
    public class StoreTimeline : ITimeline
    {
        private readonly ITweetStore _tweetStore;
        private IDisposable _updateSubscription;

        public StoreTimeline(ITweetStore tweetStore)
        {
            _tweetStore = tweetStore;
            Tweets = new ObservableCollection<Tweet>(tweetStore.GetInboxTweets());
            _updateSubscription = Observable.FromEvent<EventArgs>(handler => tweetStore.Updated += handler,
                                                                  handler => tweetStore.Updated -= handler).
                Subscribe(x => UpdateTweets());
        }

        #region ITimeline Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Tweet> Tweets { get; private set; }

        #endregion

        private void UpdateTweets()
        {
            foreach (Tweet tweet in _tweetStore.GetInboxTweets())
                Tweets.Add(tweet);
        }
    }
}