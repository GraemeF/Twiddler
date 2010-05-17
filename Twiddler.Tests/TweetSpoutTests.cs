using System.Diagnostics;
using System.Linq;
using TweetSharp.Twitter.Model;
using Xunit;

namespace Twiddler.Tests
{
    public class TweetSpoutTests
    {
        [Fact]
        public void TestThatSomethingHappens()
        {
            var poller = new TweetPoller();
            var test = new TweetSource(poller);

            poller.Start();

            foreach (TwitterStatus x in test.Tweets.ToEnumerable())
                Debug.WriteLine(x.ToString());
        }
    }
}