using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITimelineScreen))]
    public class TimelineScreen : ScreenConductor<ITweetScreen>.WithCollection.AllScreensActive, ITimelineScreen
    {
        private readonly Factories.TweetScreenFactory _screenFactory;
        private readonly Lazy<ITimeline> _timeline;
        private ITweetScreen _selection;
        private IObservable<Tweet> _tweetAdded;
        private IObservable<Tweet> _tweetRemoved;
        private IObservable<IEvent<NotifyCollectionChangedEventArgs>> _tweetsChanged;

        public TimelineScreen(Lazy<ITimeline> timeline, Factories.TweetScreenFactory screenFactory) : base(false)
        {
            _timeline = timeline;
            _screenFactory = screenFactory;
        }

        public ITweetScreen Selection
        {
            get { return _selection; }
            set
            {
                if (_selection != value)
                {
                    MarkSelectionAsRead();
                    _selection = value;
                    NotifyOfPropertyChange(() => Selection);
                }
            }
        }

        #region ITimelineScreen Members

        public override void Shutdown()
        {
            UnsubscribeFromTweets();

            base.Shutdown();
        }

        #endregion

        private void MarkSelectionAsRead()
        {
            if (_selection != null)
                _selection.MarkAsReadCommand.Execute(null);
        }

        private void UnsubscribeFromTweets()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            SubscribeToTweets();
        }

        private void SubscribeToTweets()
        {
            _tweetsChanged =
                Observable.FromEvent((EventHandler<NotifyCollectionChangedEventArgs> ev)
                                     => new NotifyCollectionChangedEventHandler(ev),
                                     ev => _timeline.Value.Tweets.CollectionChanged += ev,
                                     ev => _timeline.Value.Tweets.CollectionChanged -= ev);

            _tweetAdded = _timeline.Value.Tweets.
                ToObservable().
                Concat(_tweetsChanged.
                           SelectMany(c => c.EventArgs.
                                               SafeNewItems<Tweet>().
                                               ToObservable()));

            _tweetRemoved = _tweetsChanged.
                SelectMany(c => c.EventArgs.
                                    SafeOldItems<Tweet>().
                                    ToObservable());

            _tweetAdded.Subscribe(AddTweetScreen);
            _tweetRemoved.Subscribe(RemoveTweetScreen);
        }

        private void AddTweetScreen(Tweet tweet)
        {
            ITweetScreen screen = _screenFactory(tweet);
            screen.Initialize();
            this.OpenScreen(screen);
        }

        private void RemoveTweetScreen(Tweet tweet)
        {
            this.ShutdownScreen(Screens.First(x => x.Id == tweet.Id));
        }
    }
}