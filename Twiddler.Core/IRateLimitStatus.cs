using System;

namespace Twiddler.Services
{
    public interface IRateLimitStatus
    {
        int HourlyLimit { get; }
        DateTime ResetTime { get; }
        int RemainingHits { get; }
    }
}