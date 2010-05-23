using System;
using System.Collections.Generic;
using Twiddler.Models;

namespace Twiddler.Services
{
    public class NewTweetsEventArgs : EventArgs
    {
        public NewTweetsEventArgs(IEnumerable<Tweet> tweets)
        {
            Tweets = tweets;
        }

        public IEnumerable<Tweet> Tweets { get; private set; }
    }
}