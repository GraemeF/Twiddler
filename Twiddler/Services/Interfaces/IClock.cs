using System;

namespace Twiddler.Services.Interfaces
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}