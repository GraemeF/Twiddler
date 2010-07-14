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

namespace Twiddler.Tests.Services
{
    public class RequestConductorTests
    {
        private readonly Mock<ITwitterClient> _fakeClient = new Mock<ITwitterClient>();
        private readonly Mock<INewTweetFilter> _fakeFilter = new Mock<INewTweetFilter>();
        private readonly Mock<ITweetRequester> _fakeRequester = new Mock<ITweetRequester>();
        private readonly Mock<ITweetSink> _fakeSink = new Mock<ITweetSink>();
        private readonly IEnumerable<ITweet> _newTweets = new ITweet[] { A.Tweet.Build() };
        private readonly IEnumerable<ITweet> _requestedTweets = new ITweet[] { A.Tweet.Build() };
        private bool _requestCompleted;

        public RequestConductorTests()
        {
            _fakeRequester.
                Setup(x => x.RequestTweets()).
                Returns(_requestedTweets);

            _fakeFilter.
                Setup(x => x.RemoveKnownTweets(_requestedTweets)).
                Callback(() => _requestCompleted = true).
                Returns(_newTweets);
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
        public void Start_WhenTweetsArriveFromRequestor_AddsNewTweetsToSink()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            RequestConductor test = BuildDefaultTestSubject();
            test.Start(_fakeSink.Object);

            Wait.Until(() => _requestCompleted);

            _fakeSink.Verify(x => x.Add(_newTweets));
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus authorizationStatus)
        {
            _fakeClient.
                Setup(x => x.AuthorizationStatus).
                Returns(authorizationStatus);

            _fakeClient.Raise(x => x.PropertyChanged += null,
                              new PropertyChangedEventArgs("AuthorizationStatus"));
        }

        private RequestConductor BuildDefaultTestSubject()
        {
            return new RequestConductor(_fakeClient.Object, new[] {_fakeRequester.Object}, _fakeFilter.Object);
        }
    }
}