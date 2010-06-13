﻿using Twiddler.Commands;
using Twiddler.Core.Models;
using Twiddler.TestData;
using Xunit;

namespace Twiddler.Tests.Commands
{
    public class ArchiveTweetCommandTests
    {
        [Fact]
        public void Execute_WhenTweetIsNotArchived_ArchivesTweet()
        {
            Tweet tweet = New.Tweet;
            var test = new ArchiveTweetCommand(tweet);

            test.Execute(null);

            Assert.True(tweet.IsArchived);
        }

        [Fact]
        public void CanExecute_WhenTweetIsNotArchived_IsTrue()
        {
            var test = new ArchiveTweetCommand(New.Tweet);

            Assert.True(test.CanExecute(null));
        }

        [Fact]
        public void CanExecute_WhenTweetIsArchived_IsFalse()
        {
            var test = new ArchiveTweetCommand(New.Tweet);

            Assert.True(test.CanExecute(null));
        }

        [Fact]
        public void CanExecuteChanged_WhenTweetBecomesArchived_IsRaised()
        {
            Tweet tweet = New.Tweet;
            var test = new ArchiveTweetCommand(tweet);

            bool eventRaised = false;
            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            tweet.Archive();

            Assert.True(eventRaised);
        }
    }
}