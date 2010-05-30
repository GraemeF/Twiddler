using Autofac;
using Caliburn.Autofac;
using Caliburn.PresentationFramework.ApplicationModel;
using Microsoft.Practices.ServiceLocation;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public partial class App : CaliburnApplication
    {
        protected override IServiceLocator CreateContainer()
        {
            IContainer container = ConfigureContainer().Build();
            return new AutofacAdapter(container);
        }

        private ContainerBuilder ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            return builder;
        }

        protected override object CreateRootModel()
        {
            return Container.GetInstance<IShellScreen>();
        }
    }
}