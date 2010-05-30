using System;
using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class LinkScreenTests
    {
        private readonly Uri _uri = new Uri("http://graemef.com");

        [Fact]
        public void GettingUrl__ReturnsUrl()
        {
            var test = new LinkScreen(_uri);

            Assert.Equal(_uri, test.Uri);
        }
    }
}