using System;
using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;

namespace Twiddler.Models
{
    [PerRequest(typeof(ITimeline))]
    public class Timeline : ITimeline
    {
        public IObservable<ITweet> Tweets
        {
            get { throw new NotImplementedException(); }
        }
    }
}