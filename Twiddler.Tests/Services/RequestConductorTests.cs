using System;
using System.ComponentModel;
using System.Threading;
using Moq;
using TweetSharp.Twitter.Model;
using Twiddler.Core.Services;
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
        public void Start_WhenAuthorized_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            test.Start(_fakeSink.Object);
            // TODO: Get rid of Sleep
            Thread.Sleep(1000);

            _fakeRequester.Verify(x => x.RequestTweets());
        }

        [Fact]
        public void Start_WhenAuthorizationFollows_BeginsRequesting()
        {
            RequestConductor test = BuildDefaultTestSubject();

            test.Start(_fakeSink.Object);

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);
            GC.KeepAlive(test);

            // TODO: Get rid of Sleep
            Thread.Sleep(1000);

            _fakeRequester.Verify(x => x.RequestTweets());
        }

        [Fact]
        public void Start_WhenTweetsArriveFromRequestor_AddsTweetsToSink()
        {
            TwitterStatus tweet = New.Tweet;
            _fakeRequester.Setup(x => x.RequestTweets()).Returns(new[] {tweet});

            ClientAuthorizationStatusChangesTo(AuthorizationStatus.Authorized);

            RequestConductor test = BuildDefaultTestSubject();
            test.Start(_fakeSink.Object);

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
            return new RequestConductor(_fakeClient.Object, new[] {_fakeRequester.Object});
        }
    }
}