﻿using System.Collections.Generic;
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
        public MentionsTweetRequester(ITwitterClientFactory clientFactory,
                                      IRequestLimitStatus requestLimitStatus,
                                      Factories.TweetFactory tweetFactory)
            : base(clientFactory, requestLimitStatus, tweetFactory)
        {
        }

        protected override IEnumerable<TwitterStatus> GetStatuses(TwitterService service, long since)
        {
            return since > 0L
                       ? service.ListTweetsMentioningMeSince(since)
                       : service.ListTweetsMentioningMe();
        }
    }
}