using System;
using System.ComponentModel.Composition;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Core.Models;

namespace Twiddler.Commands
{
    [Export(typeof (IArchiveTweetCommand))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ArchiveTweetCommand : IArchiveTweetCommand
    {
        private readonly Tweet _tweet;
        private PropertyObserver<Tweet> _observer;

        [ImportingConstructor]
        public ArchiveTweetCommand(Tweet tweet)
        {
            _tweet = tweet;
            _observer = new PropertyObserver<Tweet>(_tweet).
                RegisterHandler(x => x.IsArchived,
                                x => CanExecuteChanged(this, EventArgs.Empty));
        }

        #region IArchiveTweetCommand Members

        public void Execute(object parameter)
        {
            _tweet.Archive();
        }

        public bool CanExecute(object parameter)
        {
            return !_tweet.IsArchived;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}