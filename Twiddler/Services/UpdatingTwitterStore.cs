using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (IUpdatingTweetStore))]
    public class UpdatingTwitterStore : IUpdatingTweetStore
    {
        private readonly IRequestConductor _requestConductor;
        private readonly ITweetStore _store;
        private readonly Subject<string> _newTweets = new Subject<string>();

        public UpdatingTwitterStore(IRequestConductor requestConductor, ITweetStore store)
        {
            _requestConductor = requestConductor;
            _store = store;

            _requestConductor.Start(this);
        }

        #region IUpdatingTweetStore Members

        public IObservable<string> NewTweets
        {
            get { return _newTweets; }
        }

        public bool AddTweet(Tweet tweet)
        {
            if (_store.AddTweet(tweet))
            {
                _newTweets.OnNext(tweet.Id);
                return true;
            }
            return false;
        }

        public Tweet GetTweet(string id)
        {
            return _store.GetTweet(id);
        }

        #endregion
    }
}