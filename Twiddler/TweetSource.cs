using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp.Twitter.Model;

namespace Twiddler
{
    public class TweetSource
    {
        private readonly IObservable<IEvent<NewTweetsEventArgs>> _newTweetsObserver;
        private readonly TweetPoller _poller;

        public TweetSource(TweetPoller poller)
        {
            _poller = poller;

            _newTweetsObserver =
                Observable.FromEvent((EventHandler<NewTweetsEventArgs> ev) => new EventHandler<NewTweetsEventArgs>(ev),
                                     ev => _poller.NewTweets += ev,
                                     ev => _poller.NewTweets -= ev);

            Tweets = _newTweetsObserver.SelectMany(x => x.EventArgs.Tweets);
        }

        public IObservable<TwitterStatus> Tweets { get; private set; }
    }
}