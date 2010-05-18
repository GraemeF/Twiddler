using System;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;

namespace Twiddler
{
    public class TweetPoller
    {
        private IFluentTwitter _twitter;

        public void Start()
        {
            _twitter = FluentTwitter.CreateRequest()
                .Statuses().OnPublicTimeline()
                .Configuration.UseRateLimiting(20.Percent())
                .RepeatEvery(5.Seconds()); // initial value, will be adjusted automatically
            _twitter.CallbackTo(GotTweets);
            _twitter.BeginRequest(); //recurring tasks must be async
        }

        //public void Start2()
        //{
        //    // Get the public timeline
        //    _twitter = FluentTwitter.CreateRequest().
        //        Statuses().
        //        OnPublicTimeline().
        //        AsJson().
        //        Configuration.
        //        UseRateLimiting(20.0.Percent()).
        //        RepeatEvery(5.Seconds()).
        //        CallbackTo(GotTweets);

        //    // Sequential call for data  
        //    _twitter.BeginRequest();
        //}

        public void Stop()
        {
            _twitter.Cancel();
        }

        public event EventHandler<NewTweetsEventArgs> NewTweets = delegate { };

        private void GotTweets(object sender, TwitterResult result, object userstate)
        {
            if (!result.SkippedDueToRateLimiting)
                NewTweets(this, new NewTweetsEventArgs(result.AsStatuses()));
        }
    }
}