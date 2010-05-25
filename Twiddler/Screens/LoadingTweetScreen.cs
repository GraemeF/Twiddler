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
        private readonly IUpdatingTweetStore _store;
        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        public LoadingTweetScreen(IUpdatingTweetStore store, TweetId id, Factories.TweetScreenFactory tweetScreenFactory)
        {
            _store = store;
            _id = id;
            _tweetScreenFactory = tweetScreenFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Observable.
                Start(() => _store.GetTweet(_id)).
                Subscribe(PopulateWithTweet);
        }

        private void PopulateWithTweet(Tweet tweet)
        {
            OpenScreen(_tweetScreenFactory(tweet), delegate { });
        }
    }
}