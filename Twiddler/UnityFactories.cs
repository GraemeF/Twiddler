using System;
using Microsoft.Practices.Unity;
using Twiddler.Core;

namespace Twiddler
{
    public class UnityFactories : Factories
    {
        private readonly IUnityContainer _container;

        public UnityFactories(IUnityContainer container)
        {
            _container = container;
        }

        protected override void RegisterFactory<TFactory>(TFactory factory)
        {
            _container.RegisterInstance(factory);
        }

        protected override TPart ComposePartWith<TPart, TImport>(TImport import)
        {
            IUnityContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance(import);

            return childContainer.Resolve<TPart>();
        }
    }
}