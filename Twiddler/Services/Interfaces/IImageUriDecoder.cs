namespace Twiddler.Services.Interfaces
{
    #region Using Directives

    using System;

    using Twiddler.Models;

    #endregion

    public interface IImageUriDecoder
    {
        bool CanGetImageLocations(Uri uri);

        ImageLocations GetImageLocations(Uri uri);
    }
}