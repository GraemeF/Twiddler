using System;
using System.Collections.Generic;
using Twiddler.Models.Interfaces;

namespace Twiddler.Services
{
    public class NewTweetsEventArgs : EventArgs
    {
        public NewTweetsEventArgs(IEnumerable<ITweet> tweets)
        {
            Tweets = tweets;
        }

        public IEnumerable<ITweet> Tweets { get; private set; }
    }
}