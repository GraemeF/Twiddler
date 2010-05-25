﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITimelineScreen))]
    public class TimelineScreen : ScreenConductor<ILoadingTweetScreen>.WithCollection.AllScreensActive, ITimelineScreen
    {
        private readonly Factories.LoadingTweetScreenFactory _screenFactory;
        private readonly ITimeline _timeline;
        private IObservable<TweetId> _tweetAdded;
        private IObservable<TweetId> _tweetRemoved;
        private IObservable<IEvent<NotifyCollectionChangedEventArgs>> _tweetsChanged;

        public TimelineScreen(ITimeline timeline, Factories.LoadingTweetScreenFactory screenFactory) : base(false)
        {
            _timeline = timeline;
            _screenFactory = screenFactory;
        }

        #region ITimelineScreen Members

        public override void Shutdown()
        {
            UnsubscribeFromTweets();

            base.Shutdown();
        }

        #endregion

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
                                     ev => _timeline.Tweets.CollectionChanged += ev,
                                     ev => _timeline.Tweets.CollectionChanged -= ev);

            _tweetAdded = _tweetsChanged.
                SelectMany(c => c.EventArgs.NewItems.Cast<TweetId>().ToObservable());

            _tweetRemoved = _tweetsChanged.
                SelectMany(c => c.EventArgs.OldItems.Cast<TweetId>().ToObservable());

            _tweetAdded.Subscribe(AddTweetScreen);
        }

        private void AddTweetScreen(TweetId id)
        {
            this.OpenScreen(_screenFactory(id));
        }
    }
}