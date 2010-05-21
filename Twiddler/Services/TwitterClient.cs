using System;
using System.ComponentModel;
using Caliburn.Core.IoC;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
        public AuthorizationStatus AuthorizationStatus
        {
            get { throw new NotImplementedException(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}