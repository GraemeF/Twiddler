﻿using System.ComponentModel;
using System.Threading;
using Caliburn.Testability.Extensions;
using Moq;
using Twiddler.Commands.Interfaces;
using Twiddler.Screens;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;
using Xunit;
using Xunit.Extensions;

namespace Twiddler.Tests.Screens
{
    public class StatusScreenTests
    {
        private readonly IAuthorizeCommand _authorizeCommand = new Mock<IAuthorizeCommand>().Object;
        private readonly IDeauthorizeCommand _deauthorizeCommand = new Mock<IDeauthorizeCommand>().Object;
        private readonly Mock<ITwitterClient> _fakeClient = new Mock<ITwitterClient>();
        private readonly Mock<IRequestMeterScreen> _fakeRequestMeter = new Mock<IRequestMeterScreen>();

        [Theory]
        [InlineData(AuthorizationStatus.Unknown)]
        [InlineData(AuthorizationStatus.Verifying)]
        [InlineData(AuthorizationStatus.Authorized)]
        [InlineData(AuthorizationStatus.NotAuthorized)]
        public void GettingAuthorization__ReturnsStatusFromClient(AuthorizationStatus status)
        {
            ClientAuthorizationStatusChangesTo(status);

            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Equal(status, test.Authorization);
        }

        [Fact]
        public void Initialize__ChecksAuthorization()
        {
            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();
            Thread.Sleep(1000);
            
            _fakeClient.Verify(x => x.CheckAuthorization());
        }

        [Fact]
        public void Authorization_WhenClientStatusChanges_RaisesPropertyChanged()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.NotAuthorized);

            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();

            const AuthorizationStatus newStatus = AuthorizationStatus.Authorized;

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Authorization).
                When(() => ClientAuthorizationStatusChangesTo(newStatus));

            Assert.Equal(newStatus, test.Authorization);
        }

        [Fact]
        public void GettingAuthorizeCommand__ReturnsCommand()
        {
            StatusScreen test = BuildDefaultTestSubject();

            Assert.Same(_authorizeCommand, test.AuthorizeCommand);
        }

        [Fact]
        public void GettingDeauthorizeCommand__ReturnsCommand()
        {
            StatusScreen test = BuildDefaultTestSubject();

            Assert.Same(_deauthorizeCommand, test.DeauthorizeCommand);
        }

        [Fact]
        public void GettingRequestMeter_WhenInitialized_ReturnsInitializedRequestMeter()
        {
            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();

            Assert.Same(_fakeRequestMeter.Object, test.RequestMeter);
            _fakeRequestMeter.Verify(x => x.Initialize());
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _fakeClient.Setup(x => x.AuthorizationStatus).Returns(status);
            _fakeClient.Raise(x => x.PropertyChanged += null,
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }

        private StatusScreen BuildDefaultTestSubject()
        {
            return new StatusScreen(_fakeClient.Object, _authorizeCommand, _deauthorizeCommand, _fakeRequestMeter.Object);
        }
    }
}