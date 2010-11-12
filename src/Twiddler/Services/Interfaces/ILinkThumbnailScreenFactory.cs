using System;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Services.Interfaces
{
    public interface ILinkThumbnailScreenFactory
    {
        ILinkThumbnailScreen CreateScreenForLink(Uri url);
    }
}