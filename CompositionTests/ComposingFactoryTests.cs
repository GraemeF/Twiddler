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
        public void ComposeWith__ReturnsInstanceWithInstance()
        {
            var container =
                new CompositionContainer(new TypeCatalog(typeof (Injected),
                                                         typeof (InnerItem),
                                                         typeof (OuterItem)));

            var test = new ComposingFactory(container);

            var injectedObject = new Injected();

            Assert.Same(injectedObject, test.ComposeWith<InnerItem, Injected>(injectedObject));
        }

        [Fact]
        public void ComposeWith_GivenCatalog_ReturnsInstanceWithInstance()
        {
            var typeCatalog = new TypeCatalog(typeof (Injected),
                                              typeof (InnerItem),
                                              typeof (OuterItem));
            var container =
                new CompositionContainer(typeCatalog);

            var test = new ComposingFactory(container);

            var injectedObject = new Injected();

            IDisposable childCatalog;
            Assert.Same(injectedObject, test.ComposeWith<InnerItem>(typeCatalog, out childCatalog, injectedObject));
        }

        #region Nested type: Injected

        private class Injected
        {
        }

        #endregion

        #region Nested type: InnerItem

        [Export]
        private class InnerItem
        {
            [Import]
            public Injected InjectedObject { get; set; }
        }

        #endregion

        #region Nested type: OuterItem

        [Export]
        private class OuterItem
        {
            [Import]
            public InnerItem Inner { get; set; }
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