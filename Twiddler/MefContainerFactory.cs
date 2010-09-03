using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Caliburn.MEF;
using Microsoft.Practices.ServiceLocation;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    internal class MefContainerFactory : IContainerFactory
    {
        private readonly ComposablePartCatalog _catalog;
        private CompositionContainer _compositionContainer;

        public MefContainerFactory(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        #region IContainerFactory Members

        public IServiceLocator CreateContainer()
        {
            _compositionContainer = new CompositionContainer(_catalog);
            return new MEFAdapter(_compositionContainer);
        }

        public object CreateRootModel()
        {
            return _compositionContainer.GetExportedValue<IShellScreen>();
        }

        public void Register<T>(T args) where T : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}