using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    //[PerRequest("Public Timeline", typeof (ITweetRequester))]
    public class PublicTimelineTweetRequester : TweetRequester
    {
        public PublicTimelineTweetRequester(ITwitterClient client,
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
                    OnPublicTimeline();
        }
    }
}