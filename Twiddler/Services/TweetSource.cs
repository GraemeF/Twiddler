using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetSource))]
    public class TweetSource : ITweetSource
    {
        private readonly IObservable<IEvent<NewTweetsEventArgs>> _newTweetsObserver;
        private readonly ITweetPoller _poller;

        public TweetSource(ITweetPoller poller)
        {
            _poller = poller;

            _newTweetsObserver =
                Observable.FromEvent((EventHandler<NewTweetsEventArgs> ev) => new EventHandler<NewTweetsEventArgs>(ev),
                                     ev => _poller.NewTweets += ev,
                                     ev => _poller.NewTweets -= ev);

            Tweets = _newTweetsObserver.SelectMany(x => x.EventArgs.Tweets);
        }

        #region ITweetSource Members

        public IObservable<ITweet> Tweets { get; private set; }

        #endregion
    }
}