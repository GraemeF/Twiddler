namespace Twiddler.ViewModels
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Caliburn.Micro;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.ViewModels.Interfaces;

    #endregion

    public class LoadingTweetViewModel : Conductor<IScreen>.Collection.OneActive, 
                                         ILoadingTweetScreen
    {
        private readonly ITweetPlaceholderScreen _placeholderScreen;

        private readonly ITweetStore _store;

        private readonly Factories.TweetScreenFactory _tweetScreenFactory;

        public LoadingTweetViewModel(ITweetPlaceholderScreen placeholderScreen, 
                                     ITweetStore store, 
                                     string id, 
                                     Factories.TweetScreenFactory tweetScreenFactory)
        {
            Id = id;
            _placeholderScreen = placeholderScreen;
            _store = store;
            _tweetScreenFactory = tweetScreenFactory;
        }

        public string Id { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateItem(_placeholderScreen);

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
                ActivateItem(_tweetScreenFactory(tweet));
            else
                this.CloseItem(ActiveItem);
        }
    }
}