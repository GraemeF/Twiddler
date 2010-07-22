using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Autofac;
using Caliburn.Autofac;
using Microsoft.Practices.ServiceLocation;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    internal class AutofacContainerFactory : IContainerFactory
    {
        private readonly ComposablePartCatalog _catalog;
        private IContainer _container;

        public AutofacContainerFactory(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        #region IContainerFactory Members

        public IServiceLocator CreateContainer()
        {
            _container = ConfigureContainer().Build();

            var factories = new AutofacFactories(_container);
            factories.RegisterFactories();

            return new AutofacAdapter(_container);
        }

        public object CreateRootModel()
        {
            return _container.Resolve<IShellScreen>();
        }

        #endregion

        private ContainerBuilder ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var compositionContainer = new CompositionContainer(_catalog);

            builder.RegisterInstance(compositionContainer);
            builder.RegisterInstance(compositionContainer.GetExportedValue<Core.Factories.TweetFactory>());
            builder.RegisterInstance(compositionContainer.GetExportedValue<Core.Factories.UserFactory>());

            return builder;
        }
    }
}