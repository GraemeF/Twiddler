namespace Twiddler.Services
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton(typeof(IClock))]
    [Export(typeof(IClock))]
    public class Clock : IClock
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}