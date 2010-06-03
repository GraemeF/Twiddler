using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.Screens;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, ITweetScreen
    {
        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory;
        private readonly Factories.LoadingTweetScreenFactory _loadingTweetScreenFactory;
        private readonly TwitterStatus _tweet;

        public TweetScreen(TwitterStatus tweet,
                           ILinkThumbnailScreenFactory linkThumbnailScreenFactory,
                           Factories.LoadingTweetScreenFactory loadingTweetScreenFactory)
            : base(false)
        {
            _tweet = tweet;
            _linkThumbnailScreenFactory = linkThumbnailScreenFactory;
            _loadingTweetScreenFactory = loadingTweetScreenFactory;
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

        public ILoadingTweetScreen InReplyToTweet { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            OpenLinksFromTweet();
            OpenInReplyToTweet();
        }

        private void OpenInReplyToTweet()
        {
            if (_tweet.InReplyToStatusId.HasValue)
            {
                ILoadingTweetScreen screen = _loadingTweetScreenFactory(new TweetId(_tweet.InReplyToStatusId.Value));
                screen.Initialize();
                this.OpenScreen(screen);
                InReplyToTweet = screen;
                NotifyOfPropertyChange(() => InReplyToTweet);
            }
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