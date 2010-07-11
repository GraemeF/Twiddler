using System;
using System.Threading;

namespace Twiddler.TestData
{
    public class Wait
    {
        private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(5.0);
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(0.1);

        public static void Until(Func<bool> condition)
        {
            DateTime endTime = DateTime.Now + DefaultDuration;
            while (DateTime.Now < endTime)
            {
                Thread.Sleep(DefaultPeriod);
                if (condition())
                    return;
            }

            throw new ConditionNotMetException("The condition was not met within the allowed time.");
        }
    }
}