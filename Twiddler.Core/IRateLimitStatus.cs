namespace Twiddler.Core
{
    #region Using Directives

    using System;

    #endregion

    public interface IRateLimitStatus
    {
        int HourlyLimit { get; }

        int RemainingHits { get; }

        DateTime ResetTime { get; }
    }
}