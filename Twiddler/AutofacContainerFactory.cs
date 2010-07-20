using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Autofac;
using Caliburn.Autofac;
using Microsoft.Practices.ServiceLocation;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    internal class AutofacContainerFactory : IContainerFactory
    {
        private IContainer _container;

        #region IContainerFactory Members

        public IServiceLocator CreateContainer()
        {
            _container = ConfigureContainer().Build();
            return new AutofacAdapter(_container);
        }

        public object CreateRootModel()
        {
            return _container.Resolve<IShellScreen>();
        }

        #endregion

        private static ContainerBuilder ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new CompositionContainer(new AssemblyCatalog(Assembly.GetExecutingAssembly())));
            //builder.RegisterInstance<Factories.TweetFactory>(Factories.CreateTweetFromTwitterStatus);
            //builder.RegisterInstance<Factories.UserFactory>(Factories.CreateUserFromTwitterUser);

            return builder;
        }
    }
}