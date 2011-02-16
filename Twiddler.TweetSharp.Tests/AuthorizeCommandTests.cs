namespace Twiddler.TweetSharp.Tests
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    using NSubstitute;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class AuthorizeCommandTests
    {
        private readonly IAuthorizer _authorizer = Substitute.For<IAuthorizer>();

        private readonly IAccessTokenStore _credentialsStore = Substitute.For<IAccessTokenStore>();

        private readonly ITwitterApplicationCredentials _twitterApplicationCredentials =
            Substitute.For<ITwitterApplicationCredentials>();

        [Fact]
        public void CanExecuteChanged_WhenAuthorizationStatusChanges_IsRaised()
        {
            bool eventRaised = false;

            AuthorizeCommand test = BuildDefaultTestSubject();

            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Verifying);
            GC.KeepAlive(test);

            Assert.True(eventRaised);
        }

        [Theory]
        [InlineData(AuthorizationStatus.Unauthorized)]
        public void CanExecute_WhenAbleToAuthorize_IsTrue(AuthorizationStatus status)
        {
            AuthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(status);

            Assert.True(test.CanExecute(null));
        }

        [Theory]
        [InlineData(AuthorizationStatus.Authorized)]
        [InlineData(AuthorizationStatus.InvalidApplication)]
        [InlineData(AuthorizationStatus.Verifying)]
        [InlineData(AuthorizationStatus.Unknown)]
        public void CanExecute_WhenUnableToAuthorize_IsFalse(AuthorizationStatus status)
        {
            AuthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(status);

            Assert.False(test.CanExecute(null));
        }

        private AuthorizeCommand BuildDefaultTestSubject()
        {
            return new AuthorizeCommand(_twitterApplicationCredentials, 
                                        _authorizer, 
                                        _credentialsStore);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _authorizer.AuthorizationStatus.Returns(status);
            _authorizer.PropertyChanged +=
                Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}