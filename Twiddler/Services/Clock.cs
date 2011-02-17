namespace Twiddler.Services
{
    #region Using Directives

    using System;

    using Twiddler.Services.Interfaces;

    #endregion

    public class Clock : IClock
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}