using System;

namespace Twiddler.Models
{
    public struct TweetId
    {
        private readonly long _id;

        public TweetId(long id)
        {
            _id = id;
        }

        public override bool Equals(Object obj)
        {
            return obj is TweetId && this == (TweetId) obj;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public static bool operator ==(TweetId x, TweetId y)
        {
            return x._id == y._id;
        }

        public static bool operator !=(TweetId x, TweetId y)
        {
            return !(x == y);
        }
    }
}