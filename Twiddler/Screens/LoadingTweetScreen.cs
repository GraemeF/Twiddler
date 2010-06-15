using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
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
        private readonly ISelfUpdatingTweetStore _store;
        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        public LoadingTweetScreen(ITweetPlaceholderScreen placeholderScreen,
                                  ISelfUpdatingTweetStore store,
                                  string id,
                                  Factories.TweetScreenFactory tweetScreenFactory)
        {
            _placeholderScreen = placeholderScreen;
            _store = store;
            Id = id;
            _tweetScreenFactory = tweetScreenFactory;
        }

        public string Id { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            OpenScreen(_placeholderScreen, delegate { });

            Observable.
                Start(() => GetTweet()).
                Subscribe(PopulateWithTweet);
        }

        private Tweet GetTweet()
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

        private void PopulateWithTweet(Tweet tweet)
        {
            if (tweet != null)
                OpenScreen(_tweetScreenFactory(tweet), delegate { });
            else
                this.ShutdownActiveScreen();
        }
    }
}