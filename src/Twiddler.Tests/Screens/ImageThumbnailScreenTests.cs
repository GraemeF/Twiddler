using System;
using Moq;
using Twiddler.Commands.Interfaces;
using Twiddler.Models;
using Twiddler.Screens;
using Xunit;

namespace Twiddler.Tests.Screens
{
    public class ImageThumbnailScreenTests
    {
        private readonly ImageLocations _imageLocations = new ImageLocations
                                                              {
                                                                  FullSize = new Uri("http://full.size.image"),
                                                                  Link = new Uri("http://link"),
                                                                  Thumbnail = new Uri("http://thumbnail")
                                                              };

        private readonly IOpenLinkCommand _openLinkCommand = new Mock<IOpenLinkCommand>().Object;

        [Fact]
        public void GettingOpenCommand__ReturnsCommand()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            Assert.Same(_openLinkCommand, test.OpenLinkCommand);
        }

        [Fact]
        public void GettingLink__ReturnsLinkFromLocations()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            Assert.Equal(_imageLocations.Link, test.Link);
        }

        [Fact]
        public void GettingThumbnail__ReturnsThumbnailFromLocations()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            Assert.Equal(_imageLocations.Thumbnail, test.Thumbnail);
        }

        [Fact]
        public void GettingFullSize__ReturnsFullSizeFromLocations()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            Assert.Equal(_imageLocations.FullSize, test.FullSize);
        }

        private ImageThumbnailScreen BuildDefaultTestSubject()
        {
            return new ImageThumbnailScreen(_imageLocations, _openLinkCommand);
        }
    }
}