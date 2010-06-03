using System;

namespace Twiddler.Core.Models
{
    public struct UserId
    {
        private readonly int _id;

        public UserId(int id)
        {
            _id = id;
        }

        public override bool Equals(Object obj)
        {
            return obj is UserId && this == (UserId) obj;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public static bool operator ==(UserId x, UserId y)
        {
            return x._id == y._id;
        }

        public static bool operator !=(UserId x, UserId y)
        {
            return !(x == y);
        }
    }
}