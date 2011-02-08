namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System;

    using Twiddler.Screens.Interfaces;

    #endregion

    public interface ILinkThumbnailScreenFactory
    {
        ILinkThumbnailScreen CreateScreenForLink(Uri url);
    }
}