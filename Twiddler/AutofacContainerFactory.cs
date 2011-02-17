namespace Twiddler
{
    #region Using Directives

    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Reflection;

    using Autofac;

    using Caliburn.Autofac;

    using Microsoft.Practices.ServiceLocation;

    using Twiddler.Core.Services;
    using Twiddler.Screens.Interfaces;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;
    using Twiddler.TweetSharp;
    using Twiddler.TweetSharp.TweetRequesters;
    using Twiddler.TwitterStore;
    using Twiddler.TwitterStore.Interfaces;

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

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(TweetSharp.Factories).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(TwitterDocumentStore).Assembly).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(TweetRequester).Assembly).
            //    Where(x => x.BaseType == typeof(TweetRequester)).
            //    As<ITweetRequester>();

            builder.RegisterType<Authorizer>().As<IAuthorizer>().SingleInstance();
            builder.RegisterType<TwitterClientFactory>().As<ITwitterClientFactory>().SingleInstance();
            builder.RegisterType<AccessTokenDocumentStore>().As<IAccessTokenStore>().SingleInstance();
            builder.RegisterType<DocumentStoreFactory>().As<IDocumentStoreFactory>().SingleInstance();
            builder.RegisterType<TwitterDocumentStore>().As<ITweetStore>().SingleInstance();
            builder.RegisterType<RequestLimitStatus>().As<IRequestLimitStatus>().SingleInstance();
            builder.RegisterType<StoreTimeline>().As<ITimeline>().SingleInstance();
            builder.RegisterType<LinkThumbnailScreenFactory>().As<ILinkThumbnailScreenFactory>().SingleInstance();

            var compositionContainer = new CompositionContainer(_catalog);

            builder.RegisterInstance(compositionContainer);
            builder.RegisterInstance(compositionContainer.GetExportedValue<TweetSharp.Factories.TweetFactory>());
            builder.RegisterInstance(compositionContainer.GetExportedValue<TweetSharp.Factories.UserFactory>());

            return builder;
        }
    }
}