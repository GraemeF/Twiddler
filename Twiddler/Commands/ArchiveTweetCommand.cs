namespace Twiddler.Commands
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using MvvmFoundation.Wpf;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Models;

    #endregion

    [PerRequest(typeof(IArchiveTweetCommand))]
    [Export(typeof(IArchiveTweetCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ArchiveTweetCommand : IArchiveTweetCommand
    {
        private readonly ITweet _tweet;

        private PropertyObserver<ITweet> _observer;

        [ImportingConstructor]
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