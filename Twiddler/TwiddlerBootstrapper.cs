namespace Twiddler
{
    #region Using Directives

    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    using Autofac;

    using Twiddler.Core.Services;
    using Twiddler.ViewModels.Interfaces;
    using Twiddler.Services;
    using Twiddler.Services.Interfaces;
    using Twiddler.TweetSharp;
    using Twiddler.TweetSharp.TweetRequesters;
    using Twiddler.TwitterStore;
    using Twiddler.TwitterStore.Interfaces;

    #endregion

    public class TwiddlerBootstrapper : TypedAutofacBootStrapper<IShellScreen>
    {
        private static readonly DirectoryCatalog DirectoryCatalog =
            new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Twiddler.*.dll");

        private static readonly AggregateCatalog Catalog =
            new AggregateCatalog(new ComposablePartCatalog[]
                                     {
                                         new AssemblyCatalog(Assembly.GetExecutingAssembly()), 
                                         DirectoryCatalog
                                     });

        public void Register<T>(T args) where T : class
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(args);

            builder.Update(Container);
        }

        protected override void Configure()
        {
            base.Configure();

            var factories = new AutofacFactories(Container);
            factories.RegisterFactories();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(TweetSharp.Factories).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(TwitterDocumentStore).Assembly).AsImplementedInterfaces();

            builder.RegisterType<Authorizer>().As<IAuthorizer>().SingleInstance();
            builder.RegisterType<TwitterClientFactory>().As<ITwitterClientFactory>().SingleInstance();
            builder.RegisterType<AccessTokenDocumentStore>().As<IAccessTokenStore>().SingleInstance();
            builder.RegisterType<DocumentStoreFactory>().As<IDocumentStoreFactory>().SingleInstance();
            builder.RegisterType<TwitterDocumentStore>().As<ITweetStore>().SingleInstance();
            builder.RegisterType<RequestLimitStatus>().As<IRequestLimitStatus>().SingleInstance();
            builder.RegisterType<StoreTimeline>().As<ITimeline>().SingleInstance();
            builder.RegisterType<LinkThumbnailScreenFactory>().As<ILinkThumbnailScreenFactory>().SingleInstance();

            var compositionContainer = new CompositionContainer(Catalog);

            builder.RegisterInstance(compositionContainer);
            builder.RegisterInstance(compositionContainer.GetExportedValue<TweetSharp.Factories.TweetFactory>());
            builder.RegisterInstance(compositionContainer.GetExportedValue<TweetSharp.Factories.UserFactory>());
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Register(e);
            base.OnStartup(sender, e);
        }
    }
}