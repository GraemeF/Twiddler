using System;
using TweetSharp.Twitter.Fluent;
using Twiddler.Core;
using Twiddler.Core.Services;

namespace Twiddler.TweetSharp
{
    public class TweetSharpRequest : ITwitterRequest
    {
        public TweetSharpRequest(IFluentTwitter fluentTwitter)
        {
            throw new NotImplementedException();
        }

        #region ITwitterRequest Members

        public ITwitterResult GetResponse()
        {
            throw new NotImplementedException();
        }

        #endregion

        public void AuthenticateWith(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            throw new NotImplementedException();
        }
    }
}