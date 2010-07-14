using System;
using System.ComponentModel.Composition;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Commands
{
    [Export(typeof (IMarkTweetAsReadCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MarkTweetAsReadCommand : IMarkTweetAsReadCommand
    {
        private readonly ITweetStore _store;
        private readonly ITweet _tweet;
        private PropertyObserver<ITweet> _observer;

        [ImportingConstructor]
        public MarkTweetAsReadCommand(ITweet tweet, ITweetStore store)
        {
            _tweet = tweet;
            _store = store;
            _observer = new PropertyObserver<ITweet>(_tweet).
                RegisterHandler(x => x.IsRead,
                                x => CanExecuteChanged(this, EventArgs.Empty));
        }

        #region IMarkTweetAsReadCommand Members

        public void Execute(object parameter)
        {
            _tweet.MarkAsRead();
            _store.Add(_tweet);
        }

        public bool CanExecute(object parameter)
        {
            return !_tweet.IsRead;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}