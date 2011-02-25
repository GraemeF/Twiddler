namespace ReactiveUI.Testing
{
    #region Using Directives

    using System;
    using System.Windows.Threading;

    #endregion

    public static class TestDispatcher
    {
        public static void PushFrame()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, 
                                                     new Action(() => frame.Continue = false));
            Dispatcher.PushFrame(frame);
        }
    }
}