using System;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITimeline))]
    public class StoreTimeline : ITimeline
    {
        #region ITimeline Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<string> Tweets
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}