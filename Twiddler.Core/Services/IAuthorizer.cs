namespace Twiddler.Core.Services
{
    #region Using Directives

    using ReactiveUI;

    using Twiddler.Core.Models;

    #endregion

    public interface IAuthorizer : IReactiveNotifyPropertyChanged
    {
        User AuthenticatedUser { get; }

        AuthorizationStatus AuthorizationStatus { get; }

        void CheckAuthorization();

        void Deauthorize();
    }
}