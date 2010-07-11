﻿using System;
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
        private readonly Mock<ITweetRequester> _fakeRequester = new Mock<ITweetRequester>();
        private readonly Mock<ITweetSink> _fakeSink = new Mock<ITweetSink>();
        private readonly IEnumerable<Tweet> _tweets = new Tweet[] {New.Tweet};
        private bool _requestCompleted;

        public RequestConductorTests()
        {
            _fakeRequester.
                Setup(x => x.RequestTweets()).
                Callback(() => _requestCompleted = true).
                Returns(_tweets);
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
        public void Start_WhenTweetsArriveFromRequestor_AddsTweetsToSink()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            RequestConductor test = BuildDefaultTestSubject();
            test.Start(_fakeSink.Object);

            Wait.Until(() => _requestCompleted);

            _fakeSink.Verify(x => x.Add(_tweets));
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
            return new RequestConductor(_fakeClient.Object, new[] {_fakeRequester.Object});
        }
    }
}