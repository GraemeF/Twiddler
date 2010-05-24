using System;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (IUpdatingTweetStore))]
    public class UpdatingTwitterStore : IUpdatingTweetStore
    {
        private readonly ITweetSource _source;

        public UpdatingTwitterStore(ITweetSource source)
        {
            _source = source;
        }

        #region IUpdatingTweetStore Members

        public IObservable<Tweet> NewTweets
        {
            get { return _source.Tweets; }
        }

        #endregion
    }
}