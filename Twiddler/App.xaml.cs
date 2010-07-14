using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using Caliburn.MEF;
using Caliburn.PresentationFramework.ApplicationModel;
using Microsoft.Practices.ServiceLocation;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
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

        private CompositionContainer _compositionContainer;

        protected override IServiceLocator CreateContainer()
        {
            _compositionContainer = new CompositionContainer(Catalog);
            return new MEFAdapter(_compositionContainer);
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

        protected override object CreateRootModel()
        {
            return _compositionContainer.GetExportedValue<IShellScreen>();
        }
    }
}