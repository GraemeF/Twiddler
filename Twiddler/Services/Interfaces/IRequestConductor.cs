namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System;

    using Twiddler.Core.Services;

    #endregion

    public interface IRequestConductor : IDisposable
    {
        void Start(ITweetSink tweetSink);
    }
}