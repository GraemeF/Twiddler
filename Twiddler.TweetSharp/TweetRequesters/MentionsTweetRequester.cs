using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class MentionsTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public MentionsTweetRequester(ITwitterClient client,
                                      IRequestLimitStatus requestLimitStatus,
                                      Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override IEnumerable<TwitterStatus> GetStatuses(long since)
        {
            return since > 0L
                       ? Client.Service.ListTweetsMentioningMeSince(since)
                       : Client.Service.ListTweetsMentioningMe();
        }
    }
}