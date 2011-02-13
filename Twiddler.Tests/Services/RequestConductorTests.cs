namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Moq;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class RequestConductorTests
    {
        private readonly IAuthorizer _client = Mock.Of<IAuthorizer>();

        private readonly Mock<ITweetRequester> _fakeRequester = new Mock<ITweetRequester>();

        private readonly Mock<ITweetSink> _fakeSink = new Mock<ITweetSink>();

        private readonly IEnumerable<ITweet> _requestedTweets = new[] { A.Tweet.Build() };

        private bool _requestCompleted;

        public RequestConductorTests()
        {
            _fakeRequester.
                Setup(x => x.RequestTweets()).
                Returns(_requestedTweets).
                Callback(() => _requestCompleted = true);
        }

        [Fact]
        public void Start_WhenAuthorizationFollows_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            test.Start(_fakeSink.Object);

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);
            GC.KeepAlive(test);

            Wait.Until(() => _requestCompleted);

            _fakeRequester.Verify(x => x.RequestTweets());
        }

        [Fact]
        public void Start_WhenAuthorized_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            test.Start(_fakeSink.Object);
            Wait.Until(() => _requestCompleted);

            _fakeRequester.Verify(x => x.RequestTweets());
        }

        [Fact]
        public void Start_WhenTweetsArriveFromRequestor_AddsNewTweetsToSink()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            RequestConductor test = BuildDefaultTestSubject();
            test.Start(_fakeSink.Object);

            Wait.Until(() => _requestCompleted);

            _fakeSink.Verify(x => x.Add(_requestedTweets));
        }

        private RequestConductor BuildDefaultTestSubject()
        {
            return new RequestConductor(_client, new[] { _fakeRequester.Object });
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus authorizationStatus)
        {
            Mock<IAuthorizer> fakeClient = Mock.Get(_client);

            fakeClient.Setup(x => x.AuthorizationStatus).Returns(authorizationStatus);
            fakeClient.Raise(x => x.PropertyChanged += null, 
                             new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}