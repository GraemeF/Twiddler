﻿using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    public class MentionsTweetRequester : TweetRequester
    {
        public MentionsTweetRequester(ITwitterClient client,
                                      Factories.TweetFactory tweetFactory,
                                      IRequestLimitStatus requestLimitStatus) :
                                          base(client, tweetFactory, requestLimitStatus)
        {
        }

        protected override ITwitterLeafNode CreateRequest()
        {
            return
                Client.
                    MakeRequestFor().
                    Statuses().
                    Mentions();
        }
    }
}