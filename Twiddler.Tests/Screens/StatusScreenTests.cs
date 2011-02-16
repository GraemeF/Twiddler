namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System.Threading;

    using Caliburn.Testability.Extensions;

    using NSubstitute;

    using Should.Fluent;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Services;
    using Twiddler.Screens;
    using Twiddler.Screens.Interfaces;
    using Twiddler.TestData;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class StatusScreenTests
    {
        private readonly IAuthorizeCommand _authorizeCommand = Substitute.For<IAuthorizeCommand>();

        private readonly IAuthorizer _authorizer = Substitute.For<IAuthorizer>().WithReactiveProperties();

        private readonly IDeauthorizeCommand _deauthorizeCommand = Substitute.For<IDeauthorizeCommand>();

        private readonly IRequestMeterScreen _requestMeterScreen = Substitute.For<IRequestMeterScreen>();

        [Fact]
        public void Authorization_WhenClientStatusChanges_RaisesPropertyChanged()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Unauthorized);

            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();

            const AuthorizationStatus newStatus = AuthorizationStatus.Authorized;

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Authorization).
                When(() => ClientAuthorizationStatusChangesTo(newStatus));

            test.Authorization.Should().Equal(newStatus);
        }

        [Theory]
        [InlineData(AuthorizationStatus.Unknown)]
        [InlineData(AuthorizationStatus.Verifying)]
        [InlineData(AuthorizationStatus.Authorized)]
        [InlineData(AuthorizationStatus.Unauthorized)]
        public void GettingAuthorization__ReturnsStatusFromClient(AuthorizationStatus status)
        {
            ClientAuthorizationStatusChangesTo(status);

            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.Authorization.Should().Equal(status);
        }

        [Fact]
        public void GettingAuthorizeCommand__ReturnsCommand()
        {
            StatusScreen test = BuildDefaultTestSubject();

            test.AuthorizeCommand.Should().Be.SameAs(_authorizeCommand);
        }

        [Fact]
        public void GettingDeauthorizeCommand__ReturnsCommand()
        {
            StatusScreen test = BuildDefaultTestSubject();

            test.DeauthorizeCommand.Should().Be.SameAs(_deauthorizeCommand);
        }

        [Fact]
        public void GettingRequestMeter_WhenInitialized_ReturnsInitializedRequestMeter()
        {
            StatusScreen test = BuildDefaultTestSubject();
            test.Initialize();

            test.RequestMeter.Should().Be.SameAs(_requestMeterScreen);
            _requestMeterScreen.Received().Initialize();
        }

        [Fact]
        public void Initialize__ChecksAuthorization()
        {
            StatusScreen test = BuildDefaultTestSubject();
            bool clientAuthorizationChecked = false;
            _authorizer.When(x => x.CheckAuthorization()).Do(_ => clientAuthorizationChecked = true);

            test.Initialize();
            Thread.Sleep(1000);

            Wait.Until(() => clientAuthorizationChecked);
        }

        private StatusScreen BuildDefaultTestSubject()
        {
            return new StatusScreen(_authorizer, _authorizeCommand, _deauthorizeCommand, _requestMeterScreen);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _authorizer.PropertyChanges(x => x.AuthorizationStatus, status);
        }
    }
}