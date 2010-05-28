using System;
using MvvmFoundation.Wpf;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    public class RequestConductor : IRequestConductor
    {
        private readonly ITwitterClient _client;
        private PropertyObserver<ITwitterClient> _statusObserver;

        public RequestConductor(ITwitterClient client)
        {
            _client = client;

            _statusObserver = new PropertyObserver<ITwitterClient>(_client).
                RegisterHandler(x => x.AuthorizationStatus,
                                y => PollIfAuthorized());
            PollIfAuthorized();
        }

        private void PollIfAuthorized()
        {
            if (_client.AuthorizationStatus == AuthorizationStatus.Authorized)
                EnsurePolling();
            else
                EnsureNotPolling();
        }

        private void EnsureNotPolling()
        {
        }

        private void EnsurePolling()
        {
        }

        ~RequestConductor()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                EnsureNotPolling();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}