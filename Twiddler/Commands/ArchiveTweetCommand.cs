namespace Twiddler.Commands
{
    #region Using Directives

    using System;

    using MvvmFoundation.Wpf;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Models;

    #endregion

    public class ArchiveTweetCommand : IArchiveTweetCommand
    {
        private readonly ITweet _tweet;

        private PropertyObserver<ITweet> _observer;

        public ArchiveTweetCommand(ITweet tweet)
        {
            _tweet = tweet;
            _observer = new PropertyObserver<ITweet>(_tweet).
                RegisterHandler(x => x.IsArchived, 
                                x => CanExecuteChanged(this, EventArgs.Empty));
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #region ICommand members

        public bool CanExecute(object parameter)
        {
            return !_tweet.IsArchived;
        }

        public void Execute(object parameter)
        {
            _tweet.Archive();
        }

        #endregion
    }
}