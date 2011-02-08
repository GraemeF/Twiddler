namespace Twiddler.Screens.Interfaces
{
    #region Using Directives

    using System.Windows.Input;

    using Caliburn.PresentationFramework.Screens;

    #endregion

    public interface ITweetScreen : IScreen
    {
        string Id { get; }

        ICommand MarkAsReadCommand { get; }
    }
}