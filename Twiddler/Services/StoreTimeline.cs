using System;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITimeline))]
    public class StoreTimeline : ITimeline
    {
        public StoreTimeline(ITweetStore tweetStore)
        {
            Tweets = new ObservableCollection<string>();
        }

        #region ITimeline Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<string> Tweets { get; private set; }

        #endregion
    }
}