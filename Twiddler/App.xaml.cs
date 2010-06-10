using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Caliburn.Autofac;
using Caliburn.PresentationFramework.ApplicationModel;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using Raven.Client.Document;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public partial class App : CaliburnApplication
    {
        private static readonly DirectoryCatalog DirectoryCatalog =
            new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Twiddler.*.dll");

        protected override IServiceLocator CreateContainer()
        {
            IContainer container = ConfigureContainer().Build();
            return new AutofacAdapter(container);
        }

        protected override Assembly[] SelectAssemblies()
        {
            return FindAssembliesInCatalog(DirectoryCatalog).
                Union(new[] {Assembly.GetEntryAssembly()}).ToArray();
        }

        private static IEnumerable<Assembly> FindAssembliesInCatalog(DirectoryCatalog catalog)
        {
            return catalog.LoadedFiles.
                Select(GetAssembly).
                Where(assembly => assembly != null);
        }


        private static Assembly GetAssembly(string file)
        {
            try
            {
                return Assembly.LoadFile(file);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FileLoadException)
            {
                return null;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (BadImageFormatException)
            {
                return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }

        private static ContainerBuilder ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new CompositionContainer(new AssemblyCatalog(Assembly.GetExecutingAssembly())));
            builder.RegisterInstance<Factories.TweetFactory>(Factories.CreateTweetFromTwitterStatus);
            RegisterDocumentStore(builder);

            return builder;
        }

        private static void RegisterDocumentStore(ContainerBuilder builder)
        {
            var documentStore = new DocumentStore {Url = "http://localhost:8080"};
            
            documentStore.Initialize();

            builder.
                RegisterInstance<IDocumentStore>(documentStore).
                OwnedByLifetimeScope();
        }

        private static string GenerateTweetKey(object arg)
        {
            return ((TweetSharp.Twitter.Model.TwitterStatus) arg).Id.ToString(CultureInfo.InvariantCulture);
        }

        protected override object CreateRootModel()
        {
            return Container.GetInstance<IShellScreen>();
        }
    }
}