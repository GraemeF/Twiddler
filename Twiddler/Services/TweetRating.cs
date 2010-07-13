using System;
using Caliburn.Core.IoC;
using Twiddler.Core.Services;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITweetRating))]
    public class TweetRating : ITweetRating
    {
        #region ITweetRating Members

        public bool IsMention
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDirectMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}