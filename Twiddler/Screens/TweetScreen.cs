using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.Screens;
using TweetSharp.Twitter.Model;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : ScreenConductor<ILinkThumbnailScreen>, ITweetScreen
    {
        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory;
        private readonly TwitterStatus _tweet;

        public TweetScreen(TwitterStatus tweet, ILinkThumbnailScreenFactory linkThumbnailScreenFactory)
        {
            _tweet = tweet;
            _linkThumbnailScreenFactory = linkThumbnailScreenFactory;
            Links = new BindableCollection<ILinkThumbnailScreen>();
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

        public BindableCollection<ILinkThumbnailScreen> Links { get; private set; }

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
            ILinkThumbnailScreen linkScreen = _linkThumbnailScreenFactory.CreateScreenForLink(uri);

            if (linkScreen != null)
            {
                this.OpenScreen(linkScreen);
                Links.Add(linkScreen);
            }
        }
    }
}