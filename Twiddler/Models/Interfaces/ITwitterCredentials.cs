using System;

namespace Twiddler.Models.Interfaces
{
    public interface ITwitterCredentials
    {
        string TokenSecret { get; }
        string Token { get; }
        event EventHandler<EventArgs> CredentialsChanged;
    }
}