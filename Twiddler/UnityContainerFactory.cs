using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Caliburn.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Twiddler.Screens.Interfaces;
using Twiddler.Services.Interfaces;

namespace Twiddler
{
    internal class UnityContainerFactory : IContainerFactory
    {
        private readonly ComposablePartCatalog _catalog;
        private IUnityContainer _container;

        public UnityContainerFactory(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        #region IContainerFactory Members

        public IServiceLocator CreateContainer()
        {
            _container = ConfigureContainer();
            return new UnityAdapter(_container);
        }

        public object CreateRootModel()
        {
            return _container.Resolve<IShellScreen>();
        }

        public void Register<T>(T args) where T : class
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();

            var compositionContainer = new CompositionContainer(_catalog);

            container.RegisterInstance(compositionContainer);
            container.RegisterInstance(compositionContainer.GetExportedValue<Core.Factories.TweetFactory>());
            container.RegisterInstance(compositionContainer.GetExportedValue<Core.Factories.UserFactory>());
            container.RegisterInstance(new Lazy<ITimeline>(container.Resolve<ITimeline>));

            var factories = new UnityFactories(container);
            factories.RegisterFactories();

            return container;
        }
    }
}