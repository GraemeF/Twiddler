namespace Twiddler.AcceptanceTests.TestEntities
{
    #region Using Directives

    using System;
    using System.Threading;

    #endregion

    public static class Retry
    {
        public static void Until(Func<bool> test, string failMessage)
        {
            for (int remainingTries = 50; remainingTries >= 0; remainingTries--)
            {
                if (remainingTries == 0)
                    throw new TimeoutException(failMessage);

                Thread.Sleep(500);

                if (test())
                    return;
            }
        }
    }
}