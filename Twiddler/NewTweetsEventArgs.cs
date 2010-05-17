using System;
using System.Collections.Generic;
using TweetSharp.Twitter.Model;

namespace Twiddler
{
    public class NewTweetsEventArgs : EventArgs
    {
        public NewTweetsEventArgs(IEnumerable<TwitterStatus> statuses)
        {
            Tweets = statuses;
        }

        public IEnumerable<TwitterStatus> Tweets { get; private set; }
    }
}