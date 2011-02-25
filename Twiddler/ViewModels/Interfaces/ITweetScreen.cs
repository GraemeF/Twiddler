namespace Twiddler.ViewModels.Interfaces
{
    #region Using Directives

    using System.Windows.Input;

    using Caliburn.Micro;

    #endregion

    public interface ITweetScreen : IScreen
    {
        string Id { get; }

        ICommand MarkAsReadCommand { get; }
    }
}