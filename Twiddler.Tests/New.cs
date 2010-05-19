using Twiddler.Models;
using Twiddler.Models.Interfaces;
using Twiddler.Screens;

namespace Twiddler.Tests
{
    public class New
    {
        public static ITweet Tweet { get { return new Tweet(); } }
    }
}