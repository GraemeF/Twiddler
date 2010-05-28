using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("User Timeline", typeof (ITweetRequester))]
    public class UserTimelineTweetRequester : TweetRequester
    {
        public UserTimelineTweetRequester(ITwitterClient client,
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
                    OnUserTimeline();
        }
    }
}