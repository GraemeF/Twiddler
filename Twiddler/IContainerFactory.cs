namespace Twiddler
{
    #region Using Directives

    using Microsoft.Practices.ServiceLocation;

    #endregion

    internal interface IContainerFactory
    {
        IServiceLocator CreateContainer();

        object CreateRootModel();

        void Register<T>(T args) where T : class;
    }
}