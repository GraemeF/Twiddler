using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Home Timeline", typeof (ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        public HomeTimelineTweetRequester(ITwitterClient client,
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
                    OnHomeTimeline();
        }
    }
}