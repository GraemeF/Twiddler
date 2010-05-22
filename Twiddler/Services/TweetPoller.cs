using System;
using System.Linq;
using Caliburn.Core.IoC;
using TweetSharp.Extensions;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetPoller))]
    public class TweetPoller : ITweetPoller
    {
        private readonly ITwitterClient _client;
        private readonly Factories.Tweet _tweetFactory;

        private IFluentTwitter _twitter;

        public TweetPoller(ITwitterClient client, Factories.Tweet tweetFactory)
        {
            _client = client;
            _tweetFactory = tweetFactory;
        }

        #region ITweetPoller Members

        public event EventHandler<NewTweetsEventArgs> NewTweets = delegate { };

        public void Start()
        {
            _twitter =
                _client.
                    MakeRequestFor().
                    Statuses().
                    OnHomeTimeline().
                    Configuration.
                    UseRateLimiting(20.Percent()).
                    RepeatEvery(25.Seconds());

            _twitter.CallbackTo(GotTweets);

            _twitter.BeginRequest();
        }

        public void Stop()
        {
            _twitter.Cancel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~TweetPoller()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Stop();
        }

        private void GotTweets(object sender, TwitterResult result, object userstate)
        {
            if (!result.SkippedDueToRateLimiting)
                NewTweets(this, new NewTweetsEventArgs(result.
                                                           AsStatuses().
                                                           Select(x => _tweetFactory(x.Text))));
        }
    }
}