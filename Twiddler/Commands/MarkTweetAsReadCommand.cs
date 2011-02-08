namespace Twiddler.Commands
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using MvvmFoundation.Wpf;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    [PerRequest(typeof(IMarkTweetAsReadCommand))]
    [Export(typeof(IMarkTweetAsReadCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MarkTweetAsReadCommand : IMarkTweetAsReadCommand
    {
        private readonly ITweet _tweet;

        private readonly ITweetSink _tweetSink;

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