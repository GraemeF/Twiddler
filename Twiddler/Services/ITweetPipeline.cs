using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    public interface ITweetPipeline : ITweetSource, ITweetSink
    {
    }
}