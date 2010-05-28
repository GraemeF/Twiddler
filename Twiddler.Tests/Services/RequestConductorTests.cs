using System;
using System.ComponentModel;
using Moq;
using Twiddler.Models;
using Twiddler.Services;
using Twiddler.Services.Interfaces;
using Xunit;

namespace Twiddler.Tests.Services
{
    public class RequestConductorTests
    {
        private readonly Mock<ITwitterClient> _fakeClient = new Mock<ITwitterClient>();
        private readonly Mock<ITweetRequester> _fakeRequester = new Mock<ITweetRequester>();
        private readonly Mock<ITweetSink> _fakeSink = new Mock<ITweetSink>();

        [Fact]
        public void Constructor_WhenAuthorized_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);
            GC.KeepAlive(test);

            _fakeRequester.Verify(x => x.RequestTweets());
        }

        [Fact]
        public void Constructor_WhenTweetsArriveFromRequestor_AddsTweetsToSink()
        {
            Tweet tweet = New.Tweet;
            _fakeRequester.Setup(x => x.RequestTweets()).Returns(new[] {tweet});

            RequestConductor test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);
            GC.KeepAlive(test);

            _fakeSink.Verify(x => x.AddTweet(tweet));
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
            return new RequestConductor(_fakeClient.Object, new[] {_fakeRequester.Object}, _fakeSink.Object);
        }
    }
}