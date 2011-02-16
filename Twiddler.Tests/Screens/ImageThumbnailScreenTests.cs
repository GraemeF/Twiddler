namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Models;
    using Twiddler.Screens;

    using Xunit;

    #endregion

    public class ImageThumbnailScreenTests
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
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            test.FullSize.Should().Equal(_imageLocations.FullSize);
        }

        [Fact]
        public void GettingLink__ReturnsLinkFromLocations()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            test.Link.Should().Equal(_imageLocations.Link);
        }

        [Fact]
        public void GettingOpenCommand__ReturnsCommand()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            test.OpenLinkCommand.Should().Be.SameAs(_openLinkCommand);
        }

        [Fact]
        public void GettingThumbnail__ReturnsThumbnailFromLocations()
        {
            ImageThumbnailScreen test = BuildDefaultTestSubject();

            test.Thumbnail.Should().Equal(_imageLocations.Thumbnail);
        }

        private ImageThumbnailScreen BuildDefaultTestSubject()
        {
            return new ImageThumbnailScreen(_imageLocations, _openLinkCommand);
        }
    }
}