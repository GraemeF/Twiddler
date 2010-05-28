using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetSource))]
    public class TweetSource : ITweetSource
    {
        private readonly IObservable<IEvent<NewTweetsEventArgs>> _newTweetsObserver;
        private readonly ITweetRequester _requester;

        public TweetSource(ITweetRequester requester)
        {
            _requester = requester;

            _newTweetsObserver =
                Observable.FromEvent((EventHandler<NewTweetsEventArgs> ev) => new EventHandler<NewTweetsEventArgs>(ev),
                                     ev => _requester.NewTweets += ev,
                                     ev => _requester.NewTweets -= ev);

            Tweets = _newTweetsObserver.SelectMany(x => x.EventArgs.Tweets);
        }

        #region ITweetSource Members

        public IObservable<Tweet> Tweets { get; private set; }

        #endregion
    }
}