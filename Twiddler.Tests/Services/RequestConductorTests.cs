namespace Twiddler.Tests.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using NSubstitute;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;
    using Twiddler.TestData;

    using Xunit;

    #endregion

    public class RequestConductorTests
    {
        private readonly IAuthorizer _client = Substitute.For<IAuthorizer>();

        private readonly ITweetRequester _tweetRequester = Substitute.For<ITweetRequester>();

        private readonly ITweetSink _tweetSink = Substitute.For<ITweetSink>();

        private readonly IEnumerable<ITweet> _requestedTweets = new[] { A.Tweet.Build() };

        private bool _requestCompleted;

        public RequestConductorTests()
        {
            _tweetRequester.RequestTweets().Returns(_ =>
                {
                    _requestCompleted = true;
                    return _requestedTweets;
                });
        }

        [Fact]
        public void Start_WhenAuthorizationFollows_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            test.Start(_tweetSink);

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);
            GC.KeepAlive(test);

            Wait.Until(() => _requestCompleted);

            _tweetRequester.Received().RequestTweets();
        }

        [Fact]
        public void Start_WhenAuthorized_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            test.Start(_tweetSink);
            Wait.Until(() => _requestCompleted);

            _tweetRequester.Received().RequestTweets();
        }

        [Fact]
        public void Start_WhenTweetsArriveFromRequestor_AddsNewTweetsToSink()
        {
            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            RequestConductor test = BuildDefaultTestSubject();
            test.Start(_tweetSink);

            Wait.Until(() => _requestCompleted);

            _tweetSink.Received().Add(_requestedTweets);
        }

        private RequestConductor BuildDefaultTestSubject()
        {
            return new RequestConductor(_client, new[] { _tweetRequester });
        }

        private void ClientAuthorizationStatusChangesTo(AuthorizationStatus authorizationStatus)
        {
            _client.AuthorizationStatus.Returns(authorizationStatus);
            _client.PropertyChanged +=
                Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs("AuthorizationStatus"));
        }
    }
}