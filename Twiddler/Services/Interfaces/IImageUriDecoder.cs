using System;
using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface IImageUriDecoder
    {
        bool CanGetImageLocations(Uri uri);
        ImageLocations GetImageLocations(Uri uri);
    }
}