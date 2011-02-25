namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using System;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Models;
    using Twiddler.ViewModels;

    using Xunit;

    #endregion

    public class ImageThumbnailViewModelTests
    {
        private readonly ImageLocations _imageLocations = new ImageLocations
                                                              {
                                                                  FullSize = new Uri("http://full.size.image"), 
                                                                  Link = new Uri("http://link"), 
                                                                  Thumbnail = new Uri("http://thumbnail")
                                                              };

        private readonly IOpenLinkCommand _openLinkCommand = Substitute.For<IOpenLinkCommand>();

        [Fact]
        public void GettingFullSize__ReturnsFullSizeFromLocations()
        {
            ImageThumbnailViewModel test = BuildDefaultTestSubject();

            test.FullSize.Should().Equal(_imageLocations.FullSize);
        }

        [Fact]
        public void GettingLink__ReturnsLinkFromLocations()
        {
            ImageThumbnailViewModel test = BuildDefaultTestSubject();

            test.Link.Should().Equal(_imageLocations.Link);
        }

        [Fact]
        public void GettingOpenCommand__ReturnsCommand()
        {
            ImageThumbnailViewModel test = BuildDefaultTestSubject();

            test.OpenLinkCommand.Should().Be.SameAs(_openLinkCommand);
        }

        [Fact]
        public void GettingThumbnail__ReturnsThumbnailFromLocations()
        {
            ImageThumbnailViewModel test = BuildDefaultTestSubject();

            test.Thumbnail.Should().Equal(_imageLocations.Thumbnail);
        }

        private ImageThumbnailViewModel BuildDefaultTestSubject()
        {
            return new ImageThumbnailViewModel(_imageLocations, _openLinkCommand);
        }
    }
}