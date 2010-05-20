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
        private readonly IDisposable _subscription;
        private readonly ITweetSource _tweetSource;

        public Timeline(ITweetSource tweetSource)
        {
            _tweetSource = tweetSource;
            Tweets = new ObservableCollection<ITweet>();

            _subscription = _tweetSource.Tweets.Subscribe(x => Tweets.Add(x));
        }

        #region ITimeline Members

        public ObservableCollection<ITweet> Tweets { get; private set; }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        #endregion
    }
}