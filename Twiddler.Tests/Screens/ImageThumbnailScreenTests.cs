using Moq;
using Twiddler.Commands.Interfaces;
using Twiddler.Models;
using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class ImageThumbnailScreenTests
    {
        private readonly IOpenLinkCommand _openLinkCommand = new Mock<IOpenLinkCommand>().Object;

        [Fact]
        public void GettingOpenCommand__ReturnsCommand()
        {
            var test = BuildDefaultTestSubject();

            Assert.Same(_openLinkCommand, test.OpenLinkCommand);
        }

        private ImageThumbnailScreen BuildDefaultTestSubject()
        {
            return new ImageThumbnailScreen(new ImageLocations(), _openLinkCommand);
        }
    }
}