namespace Twiddler.Screens
{
    #region Using Directives

    using System;
    using System.Windows;
    using System.Windows.Input;

    using Caliburn.PresentationFramework;
    using Caliburn.PresentationFramework.Screens;

    using MvvmFoundation.Wpf;

    using Twiddler.Commands;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    public class TweetScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, 
                               ITweetScreen
    {
        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory;

        private readonly Factories.LoadingTweetScreenFactory _loadingTweetScreenFactory;

        private readonly ITweet _tweet;

        private readonly ITweetRating _tweetRating;

        private PropertyObserver<ITweet> _tweetObserver;

        public TweetScreen(ITweet tweet, 
                           ITweetRating tweetRating, 
                           ILinkThumbnailScreenFactory linkThumbnailScreenFactory, 
                           Factories.LoadingTweetScreenFactory loadingTweetScreenFactory, 
                           ITweetStore store)
            : base(false)
        {
            _tweet = tweet;
            _tweetRating = tweetRating;
            Id = _tweet.Id;
            MarkAsReadCommand = new MarkTweetAsReadCommand(_tweet, store);
            _linkThumbnailScreenFactory = linkThumbnailScreenFactory;
            _loadingTweetScreenFactory = loadingTweetScreenFactory;
            Links = new BindableCollection<ILinkThumbnailScreen>();
        }

        public DateTime CreatedDate
        {
            get { return _tweet.CreatedDate; }
        }

        public string Id { get; private set; }

        public ILoadingTweetScreen InReplyToTweet { get; private set; }

        public BindableCollection<ILinkThumbnailScreen> Links { get; private set; }

        public ICommand MarkAsReadCommand { get; private set; }

        public Visibility MentionVisibility
        {
            get
            {
                return _tweetRating.IsMention
                           ? Visibility.Visible
                           : Visibility.Collapsed;
            }
        }

        public double Opacity
        {
            get
            {
                return _tweet.IsRead
                           ? 0.5
                           : 1.0;
            }
        }

        public string Status
        {
            get { return _tweet.Status; }
        }

        public User User
        {
            get { return _tweet.User; }
        }

        public void MarkAsRead()
        {
            _tweet.MarkAsRead();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            OpenLinksFromTweet();
            OpenInReplyToTweet();

            _tweetObserver = new PropertyObserver<ITweet>(_tweet).
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

        private void OpenLink(Uri uri)
        {
            ILinkThumbnailScreen linkScreen = _linkThumbnailScreenFactory.CreateScreenForLink(uri);

            if (linkScreen != null)
            {
                this.OpenScreen(linkScreen);
                Links.Add(linkScreen);
            }
        }

        private void OpenLinksFromTweet()
        {
            foreach (Uri textLink in _tweet.Links)
                OpenLink(textLink);
        }
    }
}