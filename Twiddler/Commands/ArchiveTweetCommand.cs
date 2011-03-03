namespace Twiddler.Commands
{
    #region Using Directives

    using System;

    using ReactiveUI;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Models;

    #endregion

    public class ArchiveTweetCommand : IArchiveTweetCommand
    {
        private readonly ITweet _tweet;

        private IDisposable _subscription;

        public ArchiveTweetCommand(ITweet tweet)
        {
            _tweet = tweet;

            _subscription = _tweet.
                WhenAny(x => x.IsArchived, _ => true).
                Subscribe(_ => CanExecuteChanged(this, EventArgs.Empty));
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