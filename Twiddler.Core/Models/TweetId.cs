using System;
using System.Diagnostics;

namespace Twiddler.Core.Models
{
    [DebuggerDisplay("Tweet:{_id}")]
    public struct TweetId : IComparable, IComparable<TweetId>
    {
        private readonly long _id;

        public TweetId(long id)
        {
            _id = id;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((TweetId) obj);
        }

        #endregion

        #region IComparable<TweetId> Members

        public int CompareTo(TweetId other)
        {
            return _id.CompareTo(other._id);
        }

        #endregion

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

        public override string ToString()
        {
            return string.Concat("Tweet:", _id.ToString());
        }
    }
}