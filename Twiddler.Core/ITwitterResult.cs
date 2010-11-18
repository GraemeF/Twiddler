using System.Collections.Generic;
using Twiddler.Core.Services;

namespace Twiddler.Core
{
    public interface ITwitterResult
    {
        IRateLimitStatus RateLimitStatus { get; }
        bool SkippedDueToRateLimiting { get; }
        bool IsNetworkError { get; }
        bool IsServiceError { get; set; }
        bool IsTwitterError { get; }
        IEnumerable<IRawStatus> AsStatuses();
    }
}