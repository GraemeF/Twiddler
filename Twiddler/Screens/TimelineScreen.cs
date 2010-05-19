using System;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    public class TimelineScreen : ScreenConductor<ITweetScreen>.WithCollection.AllScreensActive
    {
        private readonly ITimeline _timeline;
        private readonly Func<ITweet, ITweetScreen> _screenFactory;

        public TimelineScreen(ITimeline timeline, Func<ITweet,ITweetScreen> screenFactory) : base(false)
        {
            _timeline = timeline;
            _screenFactory = screenFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _timeline.Tweets.Subscribe(AddTweetScreen);
        }

        public void AddTweetScreen(ITweet tweet)
        {
            this.OpenScreen(_screenFactory(tweet));
        }
    }
}