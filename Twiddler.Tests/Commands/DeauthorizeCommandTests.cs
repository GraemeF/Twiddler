using System;
using System.ComponentModel;
using Moq;
using Twiddler.Commands;
using Twiddler.Core.Services;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Commands
{
    public class DeauthorizeCommandTests
    {
        private readonly Mock<IAuthorizer> _fakeClient = new Mock<IAuthorizer>();

        [Fact]
        public void CanExecute_WhenAuthorized_ReturnsTrue()
        {
            DeauthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            Assert.True(test.CanExecute(null));
        }

        [Theory]
        [InlineData(AuthorizationStatus.Unauthorized)]
        [InlineData(AuthorizationStatus.InvalidApplication)]
        [InlineData(AuthorizationStatus.Unknown)]
        [InlineData(AuthorizationStatus.Verifying)]
        public void CanExecute_WhenNotAuthorized_ReturnsFalse(AuthorizationStatus status)
        {
            DeauthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(status);

            Assert.False(test.CanExecute(null));
        }

        [Fact]
        public void CanExecuteChanged_WhenAuthorizationStatusChanges_IsRaised()
        {
            bool eventRaised = false;

            DeauthorizeCommand test = BuildDefaultTestSubject();

            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Verifying);
            GC.KeepAlive(test);

            Assert.True(eventRaised);
        }

        [Fact]
        public void Execute__DeauthorizesClient()
        {
            DeauthorizeCommand test = BuildDefaultTestSubject();

            test.Execute(null);

            _fakeClient.Verify(x => x.Deauthorize());
        }

        private DeauthorizeCommand BuildDefaultTestSubject()
        {
            return new DeauthorizeCommand(_fakeClient.Object);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _fakeClient.Setup(x => x.AuthorizationStatus).Returns(status);
            _fakeClient.Raise(x => x.PropertyChanged += null,
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}