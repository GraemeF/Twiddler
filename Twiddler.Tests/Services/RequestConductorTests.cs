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
        private readonly Mock<IAuthorizer> _fakeClient = new Mock<IAuthorizer>();

        private readonly Mock<ITweetRequester> _fakeRequester = new Mock<ITweetRequester>();

        private readonly Mock<ITweetSink> _fakeSink = new Mock<ITweetSink>();

        private readonly IEnumerable<ITweet> _newTweets = new[] { A.Tweet.Build() };

        private readonly IEnumerable<ITweet> _requestedTweets = new[] { A.Tweet.Build() };

        private bool _requestCompleted;

        public RequestConductorTests()
        {
            _fakeRequester.
                Setup(x => x.RequestTweets()).
                Returns(_requestedTweets);
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

            _fakeSink.Verify(x => x.Add(_newTweets));
        }

        private RequestConductor BuildDefaultTestSubject()
        {
            return new RequestConductor(_fakeClient.Object, new[] { _fakeRequester.Object });
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus authorizationStatus)
        {
            _fakeClient.
                Setup(x => x.AuthorizationStatus).
                Returns(authorizationStatus);

            _fakeClient.Raise(x => x.PropertyChanged += null, 
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}