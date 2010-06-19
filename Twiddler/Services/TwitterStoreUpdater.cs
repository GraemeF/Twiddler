﻿using Caliburn.Core.IoC;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ITwitterStoreUpdater))]
    public class TwitterStoreUpdater : ITwitterStoreUpdater
    {
        private readonly IRequestConductor _requestConductor;
        private readonly ITweetStore _store;

        public TwitterStoreUpdater(IRequestConductor requestConductor, ITweetStore store)
        {
            _requestConductor = requestConductor;
            _store = store;
        }

        #region ITwitterStoreUpdater Members

        public void Start()
        {
            _requestConductor.Start(_store);
        }

        #endregion
    }
}