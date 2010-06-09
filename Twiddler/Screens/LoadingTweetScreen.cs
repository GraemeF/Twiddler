using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ILoadingTweetScreen))]
    public class LoadingTweetScreen : ScreenConductor<IScreen>, ILoadingTweetScreen
    {
        private readonly ITweetPlaceholderScreen _placeholderScreen;
        private readonly IUpdatingTweetStore _store;
        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        public LoadingTweetScreen(ITweetPlaceholderScreen placeholderScreen,
                                  IUpdatingTweetStore store,
                                  TweetId id,
                                  Factories.TweetScreenFactory tweetScreenFactory)
        {
            _placeholderScreen = placeholderScreen;
            _store = store;
            Id = id;
            _tweetScreenFactory = tweetScreenFactory;
        }

        public TweetId Id { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            OpenScreen(_placeholderScreen, delegate { });

            Observable.
                Start(() => GetTweet()).
                Subscribe(PopulateWithTweet);
        }

        private TwitterStatus GetTweet()
        {
            try
            {
                return _store.GetTweet(Id);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        private void PopulateWithTweet(TwitterStatus tweet)
        {
            if (tweet != null)
                OpenScreen(_tweetScreenFactory(tweet), delegate { });
            else
                this.ShutdownActiveScreen();
        }
    }
}