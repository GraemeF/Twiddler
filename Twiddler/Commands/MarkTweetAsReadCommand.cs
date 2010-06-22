﻿using System;
using Caliburn.Core.IoC;
using MvvmFoundation.Wpf;
using Twiddler.Commands.Interfaces;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Commands
{
    [PerRequest(typeof (IMarkTweetAsReadCommand))]
    public class MarkTweetAsReadCommand : IMarkTweetAsReadCommand
    {
        private readonly Tweet _tweet;
        private readonly ITweetStore _store;
        private PropertyObserver<Tweet> _observer;

        public MarkTweetAsReadCommand(Tweet tweet, ITweetStore store)
        {
            _tweet = tweet;
            _store = store;
            _observer = new PropertyObserver<Tweet>(_tweet).
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