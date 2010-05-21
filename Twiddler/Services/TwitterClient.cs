using System;
using System.ComponentModel;
using Caliburn.Core.IoC;
using Twiddler.Models.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
        public TwitterClient(ITwitterCredentials twitterCredentials)
        {
        }

        public AuthorizationStatus AuthorizationStatus
        {
            get { return Interfaces.AuthorizationStatus.Unknown; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}