using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.Screens;
using TweetSharp.Twitter.Model;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : ScreenConductor<IScreen>, ITweetScreen
    {
        private readonly Factories.LinkScreenFactory _linkScreenFactory;
        private readonly TwitterStatus _tweet;

        public TweetScreen(TwitterStatus tweet, Factories.LinkScreenFactory linkScreenFactory)
        {
            _tweet = tweet;
            _linkScreenFactory = linkScreenFactory;
            Links = new BindableCollection<ILinkScreen>();
        }

        public string Status
        {
            get { return _tweet.Text; }
        }

        public TwitterUser User
        {
            get { return _tweet.User; }
        }

        public DateTime CreatedDate
        {
            get { return _tweet.CreatedDate; }
        }

        public BindableCollection<ILinkScreen> Links { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            OpenLinksFromTweet();
        }

        private void OpenLinksFromTweet()
        {
            foreach (Uri textLink in _tweet.TextLinks)
            {
                OpenLink(textLink);
            }
        }

        private void OpenLink(Uri uri)
        {
            Links.Add(CreateInitializedLink(uri));
        }

        private ILinkScreen CreateInitializedLink(Uri uri)
        {
            ILinkScreen linkScreen = _linkScreenFactory(uri);
            linkScreen.Initialize();

            return linkScreen;
        }
    }
}