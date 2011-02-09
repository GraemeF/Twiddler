namespace Twiddler
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using Caliburn.PresentationFramework.ApplicationModel;

    using Microsoft.Practices.ServiceLocation;

    #endregion

    public partial class App : CaliburnApplication
    {
        private static readonly DirectoryCatalog DirectoryCatalog =
            new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Twiddler.*.dll");

        private static readonly AggregateCatalog Catalog =
            new AggregateCatalog(new ComposablePartCatalog[]
                                     {
                                         new AssemblyCatalog(Assembly.GetExecutingAssembly()), 
                                         DirectoryCatalog
                                     });

        private readonly IContainerFactory _containerFactory = 
            new AutofacContainerFactory(Catalog);

        protected override IServiceLocator CreateContainer()
        {
            return _containerFactory.CreateContainer();
        }

        protected override object CreateRootModel()
        {
            return _containerFactory.CreateRootModel();
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            _containerFactory.Register(args);

            base.OnStartup(args);
        }

        protected override Assembly[] SelectAssemblies()
        {
            return FindAssembliesInCatalog(DirectoryCatalog).
                Union(new[] { Assembly.GetEntryAssembly() }).ToArray();
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
    }
}