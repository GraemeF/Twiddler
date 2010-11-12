using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetRating))]
    [Export(typeof (ITweetRating))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TweetRating : ITweetRating
    {
        private readonly ITwitterClient _client;
        private readonly ITweet _tweet;
        private bool _isMention;
        private PropertyObserver<ITwitterClient> _observer;

        [ImportingConstructor]
        public TweetRating(ITwitterClient client, ITweet tweet)
        {
            _client = client;
            _tweet = tweet;

            _observer = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthenticatedUser,
                                x => UpdateIsMention());
            UpdateIsMention();
        }

        #region ITweetRating Members

        public bool IsMention
        {
            get { return _isMention; }
            private set
            {
                if (_isMention != value)
                {
                    _isMention = value;
                    PropertyChanged.Raise(x => IsMention);
                }
            }
        }

        public bool IsDirectMessage
        {
            get { throw new NotImplementedException(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

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