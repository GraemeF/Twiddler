using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest("Home Timeline", typeof (ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        public HomeTimelineTweetRequester(ITwitterClient client,
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
                    OnHomeTimeline();
        }
    }
}