using System;
using Twiddler.Core;
using Twiddler.Core.Services;

namespace Twiddler.TweetSharp
{
    public class TweetSharpRequestBuilder : ITwitterRequestBuilder
    {
        public ITwitterResult Request()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder Statuses()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder OnFriendsTimeline()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder Since(long since)
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder OnHomeTimeline()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder Mentions()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder RetweetsOfMe()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder OnUserTimeline()
        {
            throw new NotImplementedException();
        }

        public ITwitterRequestBuilder AuthenticateWith(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            throw new NotImplementedException();
        }
    }
}