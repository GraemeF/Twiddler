﻿using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class MentionsTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public MentionsTweetRequester(ITwitterClient client,
                                      IRequestLimitStatus requestLimitStatus,
                                      Core.Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterStatusMentions request = Client.
                MakeRequestFor().
                Statuses().
                Mentions();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}