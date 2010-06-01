using System.ComponentModel.Composition.Hosting;
using System.Reflection;
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

            builder.RegisterInstance(new CompositionContainer(new AssemblyCatalog(Assembly.GetExecutingAssembly())));

            return builder;
        }

        protected override object CreateRootModel()
        {
            return Container.GetInstance<IShellScreen>();
        }
    }
}