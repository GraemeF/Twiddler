﻿namespace Twiddler.TweetSharp.Tests
{
    #region Using Directives

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class AuthorizerTests
    {
        [Fact]
        public void GettingAuthorization_Initially_ReturnsUnknown()
        {
            var test = new Authorizer(Substitute.For<ITwitterApplicationCredentials>(), 
                                      Substitute.For<IAccessTokenStore>(), 
                                      x => A.User);

            test.AuthorizationStatus.Should().Equal(AuthorizationStatus.Unknown);
        }
    }
}