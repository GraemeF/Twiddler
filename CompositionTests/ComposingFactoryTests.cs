using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Xunit;

namespace CompositionTests
{
    public class ComposingFactoryTests
    {
        [Fact]
        public void ComposeWith_WhenComposedTypeImportsInjectedType_ReturnsInstanceOfComposedTypeWithInjectedInstance()
        {
            // Shouldn't need to have the File type in the catalog as we are providing the instance
            var container = new CompositionContainer(new TypeCatalog(typeof (Folder)));

            // The factory has the root container which it can use to resolve Folder
            var test = new ComposingFactory(container);

            // This is the instance of File that the Folder needs to have injected
            var file = new File();

            // Compose a Folder, providing the instance of File
            Folder composedFolder = test.ComposeWith<Folder, File>(file);

            // Check that the Folder has the correct instance of File
            Assert.Same(file, composedFolder.File);
        }

        [Fact]
        public void ComposeWith_GivenCatalog_ReturnsInstanceWithInstance()
        {
            var typeCatalog = new TypeCatalog(typeof (File),
                                              typeof (Folder),
                                              typeof (Disk));
            var container =
                new CompositionContainer(typeCatalog);

            var test = new ComposingFactory(container);

            var file = new File();

            IDisposable childCatalog;
            Assert.Same(file, test.ComposeWith<Folder>(typeCatalog, out childCatalog, file).File);
        }

        #region Nested type: Disk

        [Export]
        private class Disk
        {
            [Import]
            public Folder Folder { get; set; }
        }

        #endregion

        #region Nested type: File

        private class File
        {
        }

        #endregion

        #region Nested type: Folder

        [Export]
        private class Folder
        {
            [Import]
            public File File { get; set; }
        }

        #endregion
    }

    [Export(typeof (IComposingFactory))]
    public class ComposingFactory : IComposingFactory
    {
        private readonly CompositionContainer _container;

        [ImportingConstructor]
        public ComposingFactory(CompositionContainer container)
        {
            _container = container;
        }

        #region IComposingFactory Members

        public TPart ComposeWith<TPart, TInject>(TInject injectedObject)
        {
            var childContainer = new CompositionContainer(_container);
            childContainer.ComposeExportedValue(injectedObject);
            return childContainer.GetExportedValue<TPart>();
        }

        #endregion

        //child catalog is a filtered catalog that contains only parts that should appear in the child (including TPart)
        public TPart ComposeWith<TPart>(ComposablePartCatalog childCatalog,
                                        out IDisposable disposable,
                                        params object[] imports)
        {
            var childContainer = new CompositionContainer(childCatalog, _container);
            childContainer.ComposeParts(imports);
            disposable = childContainer;
            return childContainer.GetExportedValue<TPart>();
        }
    }

    public interface IComposingFactory
    {
        TPart ComposeWith<TPart, TInject>(TInject injectedObject);
    }
}