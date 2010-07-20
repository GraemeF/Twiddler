using Microsoft.Practices.ServiceLocation;

namespace Twiddler
{
    internal interface IContainerFactory
    {
        IServiceLocator CreateContainer();
        object CreateRootModel();
    }
}