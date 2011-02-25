namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System;

    using Twiddler.ViewModels.Interfaces;

    #endregion

    public interface ILinkThumbnailScreenFactory
    {
        ILinkThumbnailScreen CreateScreenForLink(Uri url);
    }
}