namespace Twiddler.Screens
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;

    using Caliburn.PresentationFramework.Screens;

    using Twiddler.Core;
    using Twiddler.Core.Models;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    public class TimelineScreen : ScreenConductor<ITweetScreen>.WithCollection.AllScreensActive, 
                                  ITimelineScreen
    {
        private readonly Factories.TweetScreenFactory _screenFactory;

        private readonly Lazy<ITimeline> _timeline;

        private ITweetScreen _selection;

        private IObservable<ITweet> _tweetAdded;

        private IObservable<ITweet> _tweetRemoved;

        private IObservable<IEvent<NotifyCollectionChangedEventArgs>> _tweetsChanged;

        public TimelineScreen(Lazy<ITimeline> timeline, Factories.TweetScreenFactory screenFactory)
            : base(false)
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

        #region IScreen members

        public override void Shutdown()
        {
            UnsubscribeFromTweets();

            base.Shutdown();
        }

        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();

            SubscribeToTweets();
        }

        private void AddTweetScreen(ITweet tweet)
        {
            ITweetScreen screen = _screenFactory(tweet);
            screen.Initialize();
            this.OpenScreen(screen);
        }

        private void MarkSelectionAsRead()
        {
            ITweetScreen readTweet = _selection;
            if (readTweet != null)
                new TaskFactory().StartNew(() => readTweet.MarkAsReadCommand.Execute(null));
        }

        private void RemoveTweetScreen(ITweet tweet)
        {
            this.ShutdownScreen(Screens.First(x => x.Id == tweet.Id));
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
                                               SafeNewItems<ITweet>().
                                               ToObservable()));

            _tweetRemoved = _tweetsChanged.
                SelectMany(c => c.EventArgs.
                                    SafeOldItems<ITweet>().
                                    ToObservable());

            _tweetAdded.Subscribe(AddTweetScreen);
            _tweetRemoved.Subscribe(RemoveTweetScreen);
        }

        private void UnsubscribeFromTweets()
        {
        }
    }
}