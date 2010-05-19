using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Models.Interfaces;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ITimelineScreen))]
    public class TimelineScreen : ScreenConductor<ITweetScreen>.WithCollection.AllScreensActive, ITimelineScreen
    {
        private readonly Func<ITweet, ITweetScreen> _screenFactory;
        private readonly ITimeline _timeline;
        private IDisposable _subscription;

        public TimelineScreen(ITimeline timeline, Func<ITweet, ITweetScreen> screenFactory) : base(false)
        {
            _timeline = timeline;
            _screenFactory = screenFactory;
        }

        #region ITimelineScreen Members

        public override void Shutdown()
        {
            _subscription.Dispose();

            base.Shutdown();
        }

        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _subscription = _timeline.Tweets.Subscribe(AddTweetScreen);
        }

        public void AddTweetScreen(ITweet tweet)
        {
            this.OpenScreen(_screenFactory(tweet));
        }
    }
}