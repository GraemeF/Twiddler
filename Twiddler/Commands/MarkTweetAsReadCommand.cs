using System;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IMarkTweetAsReadCommand))]
    [Export(typeof (IMarkTweetAsReadCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MarkTweetAsReadCommand : IMarkTweetAsReadCommand
    {
        private readonly ITweetSink _tweetSink;
        private readonly ITweet _tweet;
        private PropertyObserver<ITweet> _observer;

        [ImportingConstructor]
        public MarkTweetAsReadCommand(ITweet tweet, ITweetSink tweetSink)
        {
            _tweet = tweet;
            _tweetSink = tweetSink;
            _observer = new PropertyObserver<ITweet>(_tweet).
                RegisterHandler(x => x.IsRead,
                                x => CanExecuteChanged(this, EventArgs.Empty));
        }

        #region IMarkTweetAsReadCommand Members

        public void Execute(object parameter)
        {
            _tweet.MarkAsRead();
            _tweetSink.Add(_tweet);
        }

        public bool CanExecute(object parameter)
        {
            return !_tweet.IsRead;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}