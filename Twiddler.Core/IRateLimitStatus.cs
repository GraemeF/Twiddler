using System;

namespace Twiddler.Core
{
    public interface IRateLimitStatus
    {
        int HourlyLimit { get; }
        DateTime ResetTime { get; }
        int RemainingHits { get; }
    }
}