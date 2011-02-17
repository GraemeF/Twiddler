namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.Linq;

    using MvvmFoundation.Wpf;

    using ReactiveUI;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    public class TweetRating : ReactiveObject, 
                               ITweetRating
    {
        private readonly IAuthorizer _client;

        private readonly ITweet _tweet;

        private bool _IsMention;

        private PropertyObserver<IAuthorizer> _observer;

        public TweetRating(IAuthorizer client, ITweet tweet)
        {
            _client = client;
            _tweet = tweet;

            _observer = new PropertyObserver<IAuthorizer>(_client).
                RegisterHandler(x => x.AuthenticatedUser, 
                                x => UpdateIsMention());
            UpdateIsMention();
        }

        public bool IsDirectMessage
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsMention
        {
            get { return _IsMention; }
            private set { this.RaiseAndSetIfChanged(x => x.IsMention, value); }
        }

        private void UpdateIsMention()
        {
            User authenticatedUser = _client.AuthenticatedUser;
            if (authenticatedUser == null)
                return;
            if (_tweet.Mentions == null)
                return;
            if (!_tweet.Mentions.Any())
                return;
            IsMention = _tweet.Mentions.Contains(authenticatedUser.ScreenName);
        }
    }
}