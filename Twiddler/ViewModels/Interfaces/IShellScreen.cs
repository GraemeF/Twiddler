namespace Twiddler.ViewModels.Interfaces
{
    #region Using Directives

    using Caliburn.Micro;

    #endregion

    public interface IShellScreen : IConductor, 
                                    IActivate, 
                                    IDeactivate
    {
    }
}