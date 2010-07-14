using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;
using Twiddler.TwitterStore.Models;

namespace Twiddler.Services
{
    [Export(typeof (ITimeline))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class StoreTimeline : ITimeline
    {
        private readonly ITweetStore _tweetStore;
        private readonly IDisposable _updateSubscription;

        [ImportingConstructor]
        public StoreTimeline(ITweetStore tweetStore)
        {
            _tweetStore = tweetStore;
            Tweets = new ObservableCollection<ITweet>(tweetStore.GetInboxTweets());
            _updateSubscription = Observable.FromEvent<EventArgs>(handler => tweetStore.Updated += handler,
                                                                  handler => tweetStore.Updated -= handler).
                Subscribe(x => UpdateTweets());
        }

        #region ITimeline Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ObservableCollection<ITweet> Tweets { get; private set; }

        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
                _updateSubscription.Dispose();
        }

        private void UpdateTweets()
        {
            IEnumerable<ITweet> inboxTweets = _tweetStore.GetInboxTweets();

            RemoveMissingTweets(inboxTweets);

            AddNewTweets(inboxTweets);
        }

        private void AddNewTweets(IEnumerable<ITweet> inboxTweets)
        {
            List<ITweet> addedTweets =
                inboxTweets.
                    Where(inboxTweet => !Tweets.
                                             Any(tweet => tweet.Id == inboxTweet.Id)).
                    ToList();

            foreach (Tweet tweet in addedTweets)
                Tweets.Add(tweet);
        }

        private void RemoveMissingTweets(IEnumerable<ITweet> inboxTweets)
        {
            List<ITweet> removedTweets =
                Tweets.
                    Where(tweet => !inboxTweets.
                                        Any(inboxTweet => tweet.Id == inboxTweet.Id)).
                    ToList();

            foreach (ITweet tweet in removedTweets)
                Tweets.Remove(tweet);
        }
    }
}