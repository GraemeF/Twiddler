using System;
using System.Linq;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ILoadingTweetScreen))]
    public class LoadingTweetScreen : ScreenConductor<ITweetScreen>, ILoadingTweetScreen
    {
        private readonly TweetId _id;
        private readonly ITweetStore _store;
        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        public LoadingTweetScreen(ITweetStore store, TweetId id, Factories.TweetScreenFactory tweetScreenFactory)
        {
            _store = store;
            _id = id;
            _tweetScreenFactory = tweetScreenFactory;
        }

        public ITweetScreen Tweet { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Observable.
                Start(() => _store.GetTweet(_id)).
                Subscribe(PopulateWithTweet);
        }

        private void PopulateWithTweet(Tweet tweet)
        {
            Tweet = _tweetScreenFactory(tweet);
            OpenScreen(Tweet, delegate { });
        }
    }
}