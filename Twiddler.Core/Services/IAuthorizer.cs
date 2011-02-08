namespace Twiddler.Core.Services
{
    #region Using Directives

    using System.ComponentModel;

    using Twiddler.Core.Models;

    #endregion

    public interface IAuthorizer : INotifyPropertyChanged
    {
        User AuthenticatedUser { get; }

        AuthorizationStatus AuthorizationStatus { get; }

        void CheckAuthorization();

        void Deauthorize();
    }
}