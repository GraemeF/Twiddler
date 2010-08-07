using Microsoft.Practices.ServiceLocation;

namespace Twiddler
{
    internal interface IContainerFactory
    {
        IServiceLocator CreateContainer();
        object CreateRootModel();
        void Register<T>(T args) where T : class;
    }
}