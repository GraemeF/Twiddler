using System;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (IClock))]
    [Export(typeof (IClock))]
    public class Clock : IClock
    {
        #region IClock Members

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        #endregion
    }
}