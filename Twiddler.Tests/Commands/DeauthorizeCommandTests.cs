namespace Twiddler.Tests.Commands
{
    #region Using Directives

    using System;

    using Caliburn.Micro;

    using NSubstitute;

    using ReactiveUI.Testing;

    using Should.Fluent;

    using Twiddler.Commands;
    using Twiddler.Core.Services;

    using Xunit;
    using Xunit.Extensions;

    #endregion

    public class DeauthorizeCommandTests
    {
        private readonly IAuthorizer _client = Substitute.For<IAuthorizer>().WithReactiveProperties();

        [Fact]
        public void CanExecuteChanged_WhenAuthorizationStatusChanges_IsRaised()
        {
            bool eventRaised = false;

            DeauthorizeCommand test = BuildDefaultTestSubject();

            test.CanExecuteChanged += (sender, args) => eventRaised = true;

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Verifying);
            TestDispatcher.PushFrame();
            GC.KeepAlive(test);

            eventRaised.Should().Be.True();
        }

        [Fact]
        public void CanExecute_WhenAuthorized_ReturnsTrue()
        {
            DeauthorizeCommand test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            test.CanExecute(null).Should().Be.True();
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

            test.CanExecute(null).Should().Be.False();
        }

        [Fact]
        public void Execute__DeauthorizesClient()
        {
            DeauthorizeCommand test = BuildDefaultTestSubject();

            test.Execute(null);

            _client.Received().Deauthorize();
        }

        private DeauthorizeCommand BuildDefaultTestSubject()
        {
            return new DeauthorizeCommand(_client);
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus status)
        {
            _client.PropertyChanges(x => x.AuthorizationStatus, status);
        }
    }
}