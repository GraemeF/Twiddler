using System;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Core.Models;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IMarkTweetAsReadCommand))]
    public class MarkTweetAsReadCommand : IMarkTweetAsReadCommand
    {
        private readonly Tweet _tweet;
        private PropertyObserver<Tweet> _observer;

        public MarkTweetAsReadCommand(Tweet tweet)
        {
            _tweet = tweet;
            _observer = new PropertyObserver<Tweet>(_tweet).
                RegisterHandler(x => x.IsRead,
                                x => CanExecuteChanged(this, EventArgs.Empty));
        }

        #region IMarkTweetAsReadCommand Members

        public void Execute(object parameter)
        {
            _tweet.MarkAsRead();
        }

        public bool CanExecute(object parameter)
        {
            return !_tweet.IsRead;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}