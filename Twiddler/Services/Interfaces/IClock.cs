namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System;

    #endregion

    public interface IClock
    {
        DateTime Now { get; }
    }
}