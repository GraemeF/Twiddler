using System;
using System.ComponentModel.Composition;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
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