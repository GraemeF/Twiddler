namespace Twiddler
{
    #region Using Directives

    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;

    using Autofac;

    using Caliburn.Autofac;

    using Microsoft.Practices.ServiceLocation;

    using Twiddler.Screens.Interfaces;

    #endregion

    internal class AutofacContainerFactory : IContainerFactory
    {
        private readonly ComposablePartCatalog _catalog;

        private IContainer _container;

        public AutofacContainerFactory(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        #region IContainerFactory members

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

        public void Register<T>(T args) where T : class
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(args);

            builder.Update(_container);
        }

        #endregion

        private ContainerBuilder ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var compositionContainer = new CompositionContainer(_catalog);

            builder.RegisterInstance(compositionContainer);
            builder.RegisterInstance(compositionContainer.GetExportedValue<TweetSharp.Factories.TweetFactory>());
            builder.RegisterInstance(compositionContainer.GetExportedValue<TweetSharp.Factories.UserFactory>());

            return builder;
        }
    }
}