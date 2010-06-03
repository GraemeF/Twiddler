using System;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Models
{
    [PerRequest(typeof (ITimeline))]
    public class Timeline : ITimeline
    {
        private readonly IDisposable _subscription;
        private readonly IUpdatingTweetStore _store;

        public Timeline(IUpdatingTweetStore store)
        {
            _store = store;
            Tweets = new ObservableCollection<TweetId>();

            _subscription = _store.Tweets.Subscribe(x => Tweets.Add(x));
        }

        #region ITimeline Members

        public ObservableCollection<TweetId> Tweets { get; private set; }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        #endregion
    }
}