namespace Twiddler.Tests.ViewModels
{
    #region Using Directives

    using System.Threading;

    using Caliburn.Micro;

    using NSubstitute;

    using ReactiveUI.Testing;

    using Should.Fluent;

    using Twiddler.Commands.Interfaces;
    using Twiddler.Core.Commands;
    using Twiddler.Core.Services;
    using Twiddler.ViewModels;
    using Twiddler.ViewModels.Interfaces;
    using Twiddler.TestData;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class StatusViewModelTests
    {
        private readonly IAuthorizeCommand _authorizeCommand = Substitute.For<IAuthorizeCommand>();

        private readonly IAuthorizer _authorizer = Substitute.For<IAuthorizer>().WithReactiveProperties();

        private readonly IDeauthorizeCommand _deauthorizeCommand = Substitute.For<IDeauthorizeCommand>();

        private readonly IRequestMeterScreen _requestMeterScreen = Substitute.For<IRequestMeterScreen>();

        [Fact]
        public void Authorization_WhenClientStatusChanges_RaisesPropertyChanged()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Unauthorized);

            StatusViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            const AuthorizationStatus newStatus = AuthorizationStatus.Authorized;

            test.
                AssertThatChangeNotificationIsRaisedBy(x => x.Authorization, 
                                                       () => ClientAuthorizationStatusChangesTo(newStatus));

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

            StatusViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.Authorization.Should().Equal(status);
        }

        [Fact]
        public void GettingAuthorizeCommand__ReturnsCommand()
        {
            StatusViewModel test = BuildDefaultTestSubject();

            test.AuthorizeCommand.Should().Be.SameAs(_authorizeCommand);
        }

        [Fact]
        public void GettingDeauthorizeCommand__ReturnsCommand()
        {
            StatusViewModel test = BuildDefaultTestSubject();

            test.DeauthorizeCommand.Should().Be.SameAs(_deauthorizeCommand);
        }

        [Fact]
        public void GettingRequestMeter_WhenInitialized_ReturnsInitializedRequestMeter()
        {
            StatusViewModel test = BuildDefaultTestSubject();
            ((IActivate)test).Activate();

            test.RequestMeter.Should().Be.SameAs(_requestMeterScreen);
            _requestMeterScreen.Received().Activate();
        }

        [Fact]
        public void Initialize__ChecksAuthorization()
        {
            StatusViewModel test = BuildDefaultTestSubject();
            bool clientAuthorizationChecked = false;
            _authorizer.When(x => x.CheckAuthorization()).Do(_ => clientAuthorizationChecked = true);

            ((IActivate)test).Activate();
            Thread.Sleep(1000);

            Wait.Until(() => clientAuthorizationChecked);
        }

        private StatusViewModel BuildDefaultTestSubject()
        {
            return new StatusViewModel(_authorizer, _authorizeCommand, _deauthorizeCommand, _requestMeterScreen);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _authorizer.PropertyChanges(x => x.AuthorizationStatus, status);
        }
    }
}