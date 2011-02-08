namespace Twiddler.TweetSharp.Tests
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    using Moq;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class AuthorizeCommandTests
    {
        private readonly Mock<ITwitterApplicationCredentials> _fakeApplicationCredentials =
            new Mock<ITwitterApplicationCredentials>();

        private readonly Mock<IAuthorizer> _fakeClient = new Mock<IAuthorizer>();

        private readonly Mock<IAccessTokenStore> _fakeCredentialsStore = new Mock<IAccessTokenStore>();

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
            return new AuthorizeCommand(_fakeApplicationCredentials.Object, 
                                        _fakeClient.Object, 
                                        _fakeCredentialsStore.Object);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _fakeClient.Setup(x => x.AuthorizationStatus).Returns(status);
            _fakeClient.Raise(x => x.PropertyChanged += null, 
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}