using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Singleton(typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class RetweetsOfMeTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public RetweetsOfMeTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Core.Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterRetweetsOfMe request = Client.
                MakeRequestFor().
                Statuses().
                RetweetsOfMe();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}