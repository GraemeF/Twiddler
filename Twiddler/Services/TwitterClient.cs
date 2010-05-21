using Caliburn.Core.IoC;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [PerRequest(typeof (ITwitterClient))]
    public class TwitterClient : ITwitterClient
    {
    }
}