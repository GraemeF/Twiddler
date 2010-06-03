using System;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using TweetSharp.Twitter.Model;
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
                Start(() => _store.GetTweet(Id)).
                Subscribe(PopulateWithTweet);
        }

        private void PopulateWithTweet(TwitterStatus tweet)
        {
            OpenScreen(_tweetScreenFactory(tweet), delegate { });
        }
    }
}