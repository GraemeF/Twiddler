using System;
using System.ComponentModel.Composition;
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

namespace Twiddler.Screens
{
    [Export(typeof (ITweetScreen))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TweetScreen : ScreenConductor<IScreen>.WithCollection.AllScreensActive, ITweetScreen
    {
        private readonly ILinkThumbnailScreenFactory _linkThumbnailScreenFactory;
        private readonly Factories.LoadingTweetScreenFactory _loadingTweetScreenFactory;
        private readonly ITweet _tweet;
        private readonly ITweetRating _tweetRating;
        private PropertyObserver<ITweet> _tweetObserver;

        [ImportingConstructor]
        public TweetScreen(ITweet tweet,
                           Factories.TweetRatingFactory tweetRatingFactory,
                           ILinkThumbnailScreenFactory linkThumbnailScreenFactory,
                           Factories.LoadingTweetScreenFactory loadingTweetScreenFactory,
                           ITweetStore store)
            : base(false)
        {
            _tweet = tweet;
            _tweetRating = tweetRatingFactory(tweet);
            Id = _tweet.Id;
            MarkAsReadCommand = new MarkTweetAsReadCommand(_tweet, store);
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

        public Visibility MentionVisibility
        {
            get
            {
                return _tweetRating.IsMention
                           ? Visibility.Visible
                           : Visibility.Collapsed;
            }
        }

        #region ITweetScreen Members

        public ICommand MarkAsReadCommand { get; private set; }

        public string Id { get; private set; }

        #endregion

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

        private void OpenLinksFromTweet()
        {
            foreach (Uri textLink in _tweet.Links)
                OpenLink(textLink);
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