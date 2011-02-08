namespace Twiddler.Core.Services
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    public interface ITweetRating : INotifyPropertyChanged
    {
        bool IsDirectMessage { get; }

        bool IsMention { get; }
    }
}