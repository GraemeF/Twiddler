using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ILoadingTweetScreen))]
    [Export(typeof (ILoadingTweetScreen))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LoadingTweetScreen : ScreenConductor<IScreen>, ILoadingTweetScreen
    {
        private readonly ITweetPlaceholderScreen _placeholderScreen;
        private readonly ITweetStore _store;
        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        [ImportingConstructor]
        public LoadingTweetScreen(ITweetPlaceholderScreen placeholderScreen,
                                  ITweetStore store,
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

        private ITweet GetTweet()
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

        private void PopulateWithTweet(ITweet tweet)
        {
            if (tweet != null)
                OpenScreen(_tweetScreenFactory(tweet), delegate { });
            else
                this.ShutdownActiveScreen();
        }
    }
}