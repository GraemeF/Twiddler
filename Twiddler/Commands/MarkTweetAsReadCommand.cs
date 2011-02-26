namespace Twiddler.Commands
{
    #region Using Directives

    using System;

    using ReactiveUI;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    public class MarkTweetAsReadCommand : IMarkTweetAsReadCommand
    {
        private readonly ITweet _tweet;

        private readonly ITweetSink _tweetSink;

        private IDisposable _subscription;

        public MarkTweetAsReadCommand(ITweet tweet, ITweetSink tweetSink)
        {
            _tweet = tweet;
            _tweetSink = tweetSink;
            _subscription = _tweet.
                WhenAny(x => x.IsRead, _ => true).
                Subscribe(x => CanExecuteChanged(this, EventArgs.Empty));
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #region ICommand members

        public bool CanExecute(object parameter)
        {
            return !_tweet.IsRead;
        }

        public void Execute(object parameter)
        {
            _tweet.MarkAsRead();
            _tweetSink.Add(_tweet);
        }

        #endregion
    }
}