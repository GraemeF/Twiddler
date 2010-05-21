using System.ComponentModel;
using Caliburn.Testability.Extensions;
using Moq;
using Twiddler.Screens;
using Twiddler.Services.Interfaces;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Screens
{
    public class StatusScreenTests
    {
        private readonly Mock<ITwitterClient> _fakeClient = new Mock<ITwitterClient>();

        [Theory]
        [InlineData(AuthorizationStatus.Unknown)]
        [InlineData(AuthorizationStatus.Athorizing)]
        [InlineData(AuthorizationStatus.Authorized)]
        [InlineData(AuthorizationStatus.NotAuthorized)]
        public void GettingAuthorization__ReturnsStatusFromClient(AuthorizationStatus status)
        {
            ClientAuthorizationStatusChangesTo(status);

            var test = new StatusScreen(_fakeClient.Object);
            test.Initialize();

            Assert.Equal(status, test.Authorization);
        }

        [Fact]
        public void Authorization_WhenClientStatusChanges_RaisesPropertyChanged()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.NotAuthorized);

            var test = new StatusScreen(_fakeClient.Object);
            test.Initialize();

            const AuthorizationStatus newStatus = AuthorizationStatus.Authorized;

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Authorization).
                When(() => ClientAuthorizationStatusChangesTo(newStatus));

            Assert.Equal(newStatus, test.Authorization);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _fakeClient.Setup(x => x.AuthorizationStatus).Returns(status);
            _fakeClient.Raise(x => x.PropertyChanged += null,
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}