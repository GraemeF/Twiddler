using System;

namespace Twiddler.AcceptanceTests.TestEntities
{
    public class TwitterService : IDisposable
    {
        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by an instance of the <see cref="TwitterService" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in <strong>true</strong>, and then suppresses 
        /// finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Releases unmanaged resources before an instance of the <see cref="TwitterService" /> class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// This method releases unmanaged resources by calling the virtual <see cref="Dispose(bool)" /> method, passing in <strong>false</strong>.
        /// </remarks>
        ~TwitterService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the <see cref="TwitterService" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><strong>true</strong> to release both managed and unmanaged resources; <strong>false</strong> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Start()
        {
        }
    }
}