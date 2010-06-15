using System;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Models
{
    [PerRequest(typeof (ITimeline))]
    public class SubscribingTimeline : ITimeline
    {
        private readonly ISelfUpdatingTweetStore _store;
        private readonly IDisposable _subscription;

        public SubscribingTimeline(ISelfUpdatingTweetStore store)
        {
            _store = store;
            Tweets = new ObservableCollection<string>();

            _subscription = _store.InboxTweets.Subscribe(x => Tweets.Add(x));
        }

        #region ITimeline Members

        public ObservableCollection<string> Tweets { get; private set; }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        #endregion
    }
}