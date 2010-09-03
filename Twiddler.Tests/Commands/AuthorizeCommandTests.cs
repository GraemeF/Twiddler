using System;
using System.ComponentModel;
using Moq;
using Twiddler.Commands;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Commands
{
    public class AuthorizeCommandTests
    {
        private readonly Mock<ITwitterClient> _fakeClient = new Mock<ITwitterClient>();
        private readonly Mock<ICredentialsStore> _fakeCredentialsStore = new Mock<ICredentialsStore>();

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

        [Theory]
        [InlineData(AuthorizationStatus.NotAuthorized)]
        public void CanExecute_WhenAbleToAuthorize_IsTrue(AuthorizationStatus status)
        {
            AuthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(status);

            Assert.True(test.CanExecute(null));
        }

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

        private AuthorizeCommand BuildDefaultTestSubject()
        {
            return new AuthorizeCommand(_fakeClient.Object, _fakeCredentialsStore.Object);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _fakeClient.Setup(x => x.AuthorizationStatus).Returns(status);
            _fakeClient.Raise(x => x.PropertyChanged += null,
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}