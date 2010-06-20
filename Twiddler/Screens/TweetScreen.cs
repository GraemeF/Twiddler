using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.Screens;
using MvvmFoundation.Wpf;
using Twiddler.Core.Models;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITweetScreen))]
    public class TweetScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, ITweetScreen
    {
        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory;
        private readonly Factories.LoadingTweetScreenFactory _loadingTweetScreenFactory;
        private readonly Tweet _tweet;
        private PropertyObserver<Tweet> _tweetObserver;

        public TweetScreen(Tweet tweet,
                           ILinkThumbnailScreenFactory linkThumbnailScreenFactory,
                           Factories.LoadingTweetScreenFactory loadingTweetScreenFactory)
            : base(false)
        {
            _tweet = tweet;
            Id = _tweet.Id;
            _linkThumbnailScreenFactory = linkThumbnailScreenFactory;
            _loadingTweetScreenFactory = loadingTweetScreenFactory;
            Links = new BindableCollection<ILinkThumbnailScreen>();
        }

        public string Status
        {
            get { return _tweet.Status; }
        }

        public User User
        {
            get { return _tweet.User; }
        }

        public DateTime CreatedDate
        {
            get { return _tweet.CreatedDate; }
        }

        public BindableCollection<ILinkThumbnailScreen> Links { get; private set; }

        public ILoadingTweetScreen InReplyToTweet { get; private set; }

        public double Opacity
        {
            get { return _tweet.IsRead ? 0.5 : 1.0; }
        }

        #region ITweetScreen Members

        public string Id { get; private set; }

        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();
            OpenLinksFromTweet();
            OpenInReplyToTweet();

            _tweetObserver = new PropertyObserver<Tweet>(_tweet).
                RegisterHandler(x => x.IsRead,
                                x => NotifyOfPropertyChange(() => Opacity));
        }

        private void OpenInReplyToTweet()
        {
            if (_tweet.InReplyToStatusId != null)
            {
                ILoadingTweetScreen screen = _loadingTweetScreenFactory(_tweet.InReplyToStatusId);
                screen.Initialize();
                InReplyToTweet = screen;
                NotifyOfPropertyChange(() => InReplyToTweet);
            }
        }

        private void OpenLinksFromTweet()
        {
            foreach (Uri textLink in _tweet.Links)
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