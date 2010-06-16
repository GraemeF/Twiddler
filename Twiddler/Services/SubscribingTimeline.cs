using System;
using System.Collections.ObjectModel;
using Twiddler.Core.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    //[PerRequest(typeof (ITimeline))]
    public class SubscribingTimeline : ITimeline
    {
        private readonly ISelfUpdatingTweetStore _store;
        private readonly IDisposable _subscription;

        public SubscribingTimeline(ISelfUpdatingTweetStore store)
        {
            _store = store;
            Tweets = new ObservableCollection<Tweet>();

            _subscription = _store.InboxTweets.Subscribe(x => Tweets.Add(x));
        }

        #region ITimeline Members

        public ObservableCollection<Tweet> Tweets { get; private set; }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        #endregion
    }
}