using System;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    public class TimelineScreen : ScreenConductor<ITweetScreen>.WithCollection.AllScreensActive
    {
        private readonly ITimeline _timeline;
        private readonly Func<ITweet, ITweetScreen> _screenFactory;
        private IDisposable _subscription;

        public TimelineScreen(ITimeline timeline, Func<ITweet,ITweetScreen> screenFactory) : base(false)
        {
            _timeline = timeline;
            _screenFactory = screenFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _subscription = _timeline.Tweets.Subscribe(AddTweetScreen);
        }

        public override void Shutdown()
        {
            _subscription.Dispose();

            base.Shutdown();
        }

        public void AddTweetScreen(ITweet tweet)
        {
            this.OpenScreen(_screenFactory(tweet));
        }
    }
}