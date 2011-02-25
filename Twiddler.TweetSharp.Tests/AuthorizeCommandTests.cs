namespace Twiddler.TweetSharp.Tests
{
    #region Using Directives

    using System;
    using System.Windows.Threading;

    using NSubstitute;

    using ReactiveUI.Testing;

    using Should.Fluent;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class AuthorizeCommandTests
    {
        private readonly IAuthorizer _authorizer = Substitute.For<IAuthorizer>().WithReactiveProperties();

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
            TestDispatcher.PushFrame();

            GC.KeepAlive(test);

            eventRaised.Should().Be.True();
        }

        [Theory]
        [InlineData(AuthorizationStatus.Unauthorized)]
        public void CanExecute_WhenAbleToAuthorize_IsTrue(AuthorizationStatus status)
        {
            AuthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(status);

            test.CanExecute(null).Should().Be.True();
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

            test.CanExecute(null).Should().Be.False();
        }

        private AuthorizeCommand BuildDefaultTestSubject()
        {
            return new AuthorizeCommand(_twitterApplicationCredentials, 
                                        _authorizer, 
                                        _credentialsStore);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _authorizer.PropertyChanges(x => x.AuthorizationStatus, status);
        }
    }
}