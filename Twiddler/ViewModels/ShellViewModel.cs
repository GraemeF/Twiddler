namespace Twiddler.ViewModels
{
    #region Using Directives

    using Caliburn.Micro;

    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services.Interfaces;

    #endregion

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, 
                               IShellScreen
    {
        private readonly ITimelineUpdater _timelineUpdater;

        public ShellViewModel(ITimelineScreen timelineScreen, 
                           IStatusScreen statusScreen, 
                           ITimelineUpdater timelineUpdater)
            : base(true)
        {
            _timelineUpdater = timelineUpdater;
            Timeline = timelineScreen;
            Status = statusScreen;
        }

        public IStatusScreen Status { get; private set; }

        public ITimelineScreen Timeline { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateItem(Timeline);
            ActivateItem(Status);

            _timelineUpdater.Start();
        }
    }
}