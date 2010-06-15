using System;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Models
{
    [PerRequest(typeof (ITimeline))]
    public class Timeline : ITimeline
    {
        private readonly ISelfUpdatingTweetStore _store;
        private readonly IDisposable _subscription;

        public Timeline(ISelfUpdatingTweetStore store)
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