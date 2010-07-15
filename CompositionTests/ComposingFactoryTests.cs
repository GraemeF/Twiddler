using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Xunit;

namespace CompositionTests
{
    public class ComposingFactoryTests
    {
        [Fact]
        public void ComposeWith__ReturnsInstanceWithInstance()
        {
            var container =
                new CompositionContainer(new TypeCatalog(typeof(Injected),
                                                         typeof(InnerItem),
                                                         typeof(OuterItem)));

            var test = new ComposingFactory(container);

            var injectedObject = new Injected();

            Assert.Same(injectedObject, test.ComposeWith<InnerItem, Injected>(injectedObject));
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

    [Export(typeof(IComposingFactory))]
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
    }

    public interface IComposingFactory
    {
        TPart ComposeWith<TPart, TInject>(TInject injectedObject);
    }
}