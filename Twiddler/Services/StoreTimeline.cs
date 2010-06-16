using System;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITimeline))]
    public class StoreTimeline : ITimeline
    {
        public StoreTimeline(ITweetStore tweetStore)
        {
            Tweets = new ObservableCollection<Tweet>(tweetStore.GetInboxTweets());
        }

        #region ITimeline Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Tweet> Tweets { get; private set; }

        #endregion
    }
}