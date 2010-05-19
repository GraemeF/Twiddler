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
        private readonly ITweetSource _tweetSource;

        public Timeline(ITweetSource tweetSource)
        {
            _tweetSource = tweetSource;
            Tweets = new ObservableCollection<ITweet>();

            _tweetSource.Tweets.Subscribe(x => Tweets.Add(x));
        }

        #region ITimeline Members

        public ObservableCollection<ITweet> Tweets { get; private set; }

        #endregion
    }
}