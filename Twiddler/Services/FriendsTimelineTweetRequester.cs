﻿using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("Friends Timeline", typeof (ITweetRequester))]
    public class FriendsTimelineTweetRequester : TweetRequester
    {
        public FriendsTimelineTweetRequester(ITwitterClient client,
                                             IRequestLimitStatus requestLimitStatus) :
                                                 base(client, requestLimitStatus)
        {
        }

        protected override ITwitterLeafNode CreateRequest()
        {
            return
                Client.
                    MakeRequestFor().
                    Statuses().
                    OnFriendsTimeline();
        }
    }
}