namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.Linq;

    using ReactiveUI;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    public class TweetRating : ReactiveObject, 
                               ITweetRating
    {
        private readonly IAuthorizer _client;

        private readonly ITweet _tweet;

        private bool _IsDirectMessage;

        private bool _IsMention;

        private bool _IsRead;

        private IDisposable _mentionSubscription;

        private IDisposable _readSubscription;

        public TweetRating(IAuthorizer client, ITweet tweet)
        {
            _client = client;
            _tweet = tweet;

            _readSubscription = tweet.
                WhenAny(x => x.IsRead, isRead => isRead.Value).
                Subscribe(x => IsRead = x);

            _mentionSubscription = _client.
                WhenAny(x => x.AuthenticatedUser, x => IsUserMentioned(x.Value)).
                Subscribe(x => IsMention = x);
        }

        public bool IsDirectMessage
        {
            get { return _IsDirectMessage; }
            private set { this.RaiseAndSetIfChanged(x => x.IsDirectMessage, value); }
        }

        public bool IsMention
        {
            get { return _IsMention; }
            private set { this.RaiseAndSetIfChanged(x => x.IsMention, value); }
        }

        public bool IsRead
        {
            get { return _IsRead; }
            private set { this.RaiseAndSetIfChanged(x => x.IsRead, value); }
        }

        private bool IsUserMentioned(User user)
        {
            if (user == null)
                return false;
            if (_tweet.Mentions == null)
                return false;
            if (!_tweet.Mentions.Any())
                return false;
            return _tweet.Mentions.Contains(user.ScreenName);
        }
    }
}
