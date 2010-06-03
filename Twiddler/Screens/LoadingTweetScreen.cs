using System;
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
    public class LoadingTweetScreen : ScreenConductor<ITweetScreen>, ILoadingTweetScreen
    {
        private readonly IUpdatingTweetStore _store;
        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        public LoadingTweetScreen(IUpdatingTweetStore store, TweetId id, Factories.TweetScreenFactory tweetScreenFactory)
        {
            _store = store;
            Id = id;
            _tweetScreenFactory = tweetScreenFactory;
        }

        public TweetId Id { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

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